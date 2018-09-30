using UnityEngine;
using System.Collections.Generic;
    
class LandingServer: MonoBehaviour
    {
	public bool started;
        private Dictionary<int,List<int>> stationLandingBase; //StationLandingBase[station_id][ship_id]
//        private Dictionary<int, Dictionary<int, Dictionary<int, InventoryItem>>> playerSOInventoryBase; //playerStationItems[ player_id,[Inventoy_holder_id,list_of_items_in inventory]]
        
        private List<int> stationsDB;


        
        void Awake(){
            
		stationLandingBase = new Dictionary<int,List<int>>();
//            playerSOInventoryBase = new Dictionary<int, Dictionary<int, Dictionary<int, InventoryItem>>>();
        }
        void Start()
        {
            LoadData();

			started = true;

        }

        private void LoadData(){
            List<ServerObject> SOList = GetComponent<ServerDB>().GetAllSO();
            stationsDB = new List<int>();
            
            for (int i = 0; i < SOList.Count; i++)
            {
                if (SOList[i].type != ServerObject.typeSO.station)
                {
                    stationsDB.Add(SOList[i].id);
                }
            }
        }

        private void SaveData(int station_id){

        }
        public bool Landing(int station_id, int serverObject_id){
            // TODO here we can check if this ship can land the station
            for (int i=0;i<stationsDB.Count;i++){
                if (station_id == stationsDB[i]){
				if( stationLandingBase.ContainsKey(station_id)){
                    stationLandingBase[station_id].Add(serverObject_id);
                    return true;
				} else {
					stationLandingBase.Add(station_id,new List<int>());
					stationLandingBase[station_id].Add(serverObject_id);
					return true;
			}
            }
		}
            return false;
        }
        public bool TakeOff(int station_id, int serverObject_id){
            for (int i=0;i<stationsDB.Count;i++){
                if (station_id == stationsDB[i]){
                    for (int j=0;j<stationLandingBase[station_id].Count;j++){
                        if (stationLandingBase[station_id][i] ==serverObject_id){
                            stationLandingBase[station_id].RemoveAt(i);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public List<int> PlayerShipList(int station_id, int player_id)
        {
            List<int> retList=new List<int>();
            for (int i = 0; i < stationsDB.Count; i++)
            {
                if (station_id == stationsDB[i])
                {
                    if (stationLandingBase.ContainsKey(station_id)){
                            return stationLandingBase[station_id];
                        }
                 }
                
            }
            return retList;
        }

        public int FindShipLocation(int ship_id)
        {
            foreach (int i in stationLandingBase.Keys)
            {
                if (stationLandingBase[i].Contains(ship_id))
                {
                    return i;
                }
            }
            return 0;
        }
    }
