using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using UnityEngine;

public class TempPlayer {
	public int Room;
	public int Team;
	public byte PlayerSlot;
	bool mInMatch;
	public int[] partyIDs = new int[4] { 0, 0, 0, 0 };

	public string DisplayName;

	// Do Not Store
	private int PlayerIndex;
	private int PlayerChoice = 0;
	private int PlayerOptions = 0;

	int AISeed = 0;
	
	
	// Delegates
	public delegate void OnChoiceSelection(TempPlayer tPlayer, int choice);
	public event OnChoiceSelection ChoiceSelection;
	

	protected DataHolderClass TempDataHolder;

	public void Reset() {
		Room = 0;
		Team = 0;
		PlayerSlot = 0;
		mInMatch = false;
	}

	public int PartyCount() {
		int count = 0;
		for (int i = 0; i < partyIDs.Length; i++) {
			if (partyIDs[i] != 0) {
				count++;
			}
		}

		return count;
	}

	public void ResetPartyIDs() {
		for (int i = 0; i < partyIDs.Length; i++) {
			partyIDs[i] = 0;
		}
	}

	public int GetPlayerChoice() { return PlayerChoice; } // Return true if not a player
	public int GetPlayerOptions() { return PlayerOptions; } // Return true if not a player
	public void SetPlayerChoice(int choice) { 
		PlayerChoice = choice;
		ChoiceSelection?.Invoke(this, choice);
	}
	public void SetPlayerOptions(int options) { PlayerOptions = options; }
	public void ResetPlayerChoice() { PlayerChoice = (int)kTempPlayerChoices.NotReady; }
	public bool IsPlayerAI() { return PlayerIndex == 0; }

	public void RollAISeed() { AISeed = UnityEngine.Random.Range(1, int.MaxValue); }

	// For players joining Matches
	public virtual void LoadFromBuffer(BufferReadWrite buffer) {
		DisplayName = buffer.ReadString();
		Team = buffer.ReadByte();
		PlayerSlot = buffer.ReadByte();
		AISeed = buffer.ReadInteger();

		TempDataHolder.LoadHoldersFromBuffer(buffer, "TempPlayerHolder");
	}

	public virtual void LoadFromPlayerData(PlayerData playerData, int team, int pSlot) {
		DisplayName = playerData.DisplayName;
		Team = team;
		PlayerSlot = (byte)pSlot;

		BufferReadWrite buffer = new BufferReadWrite();
		playerData.SaveHoldersToBuffer(buffer, "TempPlayerHolder");

		TempDataHolder.LoadHoldersFromBuffer(buffer, "TempPlayerHolder");
	}

	public virtual void SaveToBuffer(BufferReadWrite buffer) {
		UnityEngine.Debug.LogWarningFormat("Name: {0}, Team: {1}, Slot: {2}", DisplayName, Team, PlayerSlot);
		buffer.WriteString(DisplayName);
		buffer.WriteByte((byte)Team);
		buffer.WriteByte(PlayerSlot);
		buffer.WriteInteger(AISeed);

		TempDataHolder.SaveHoldersToBuffer(buffer, "TempPlayerHolder");
	}

	// This playerData is used for local hosting
	public void InitDataHolder(PlayerData playerData = null) {
		TempDataHolder = new DataHolderClass();
		TempDataHolder.InitHolderMap(playerData, "TempPlayerHolder");
	}

	public PlayerDataHolder GetDataHolder(string holder) {
		return TempDataHolder.GetDataHolders().ContainsKey(holder) ? TempDataHolder.GetDataHolder(holder) : null;
	}

	public DataHolderClass GetDataHolderClass() { return TempDataHolder; }

	
	public int GetPlayerSlot() { return PlayerIndex; }

	public int GetPlayerServerIndex() { return PlayerIndex; }
	public void SetPlayerIndex(int index) { PlayerIndex = index; }

	public bool InMatch {
		get {
			return mInMatch;
		}
		set {
			mInMatch = value;
		}
	}
	
	public enum kTempPlayerChoices {
		NotReady = -1,
		Ready
	}
}
