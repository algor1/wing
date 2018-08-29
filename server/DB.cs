using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// Подключаем необходимые пространства имен
using System.Data;

using Mono.Data.Sqlite;
using System.IO;


public class DB : MonoBehaviour {
//	string debugtext;
	private IDbConnection dbcon;
	void Start () {
//		debugtext = GetComponent<Text> ().text;
//		debugtext += "qqqq";
		// check if file exists in Application.persistentDataPath
		string p="wing.db";

		string filepath = Application.persistentDataPath + "/" + p;
		if (File.Exists (filepath)) {
//			File.Delete (filepath);
		}

		if (!File.Exists (filepath)) {
			Debug.Log (filepath);
			// if it doesn't ->

			// open StreamingAssets directory and load the db ->

			WWW loadDB = new WWW ( "jar:file://" + Application.dataPath + "!/assets/"+ p);  // this is the path to your StreamingAssets in android
//			debugtext= loadDB.url+"\n";
//			Debug.Log (debugtext);
//			GetComponent<Text> ().text = debugtext;
			while (!loadDB.isDone) {
				Debug.Log ("LOADING..."+ loadDB.uploadProgress);
			}  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
//			Debug.Log("loaded "+loadDB.bytesDownloaded);
//			debugtext+="loaded "+loadDB.bytesDownloaded+"\n";

//			GetComponent<Text> ().text = debugtext;

			// then save to Application.persistentDataPath
			Debug.Log (loadDB.error);
//			debugtext += loadDB.error + "\n";
//			debugtext += filepath + "\n";
//			GetComponent<Text> ().text = debugtext;



			File.WriteAllBytes (filepath, loadDB.bytes);
		}

		    

		string connectionString = "URI=file:"+filepath ;
//		debugtext += filepath+" exist "+ File.Exists (filepath).ToString()+"\n";
//		GetComponent<Text> ().text = debugtext;

		dbcon = (IDbConnection)new SqliteConnection (connectionString);

//			debugtext += "try open \n";
		dbcon.Open();
			Debug.Log ("open");
//			debugtext += "open "+dbcon.State+"\n";
//			GetComponent<Text> ().text = debugtext;

			// Выбираем нужные нам данные
	

//		Debug.Log (debugtext);
//		GetComponent<Text> ().text = debugtext;

	}
	public void CloseConnection()
	{
		// Закрываем соединение

		dbcon.Close();
	}
	public IDataReader GetReader(string dbselect){

//	var sql = "SELECT * FROM skills LIMIT 1";
		using (IDbCommand dbcmd = dbcon.CreateCommand())
		{
			//				debugtext += "comm "+dbcmd.Connection.Database.ToString()+"\n";
			//				GetComponent<Text> ().text = debugtext;

			Debug.Log ("comm ");
				dbcmd.CommandText = dbselect;
			// Выполняем запрос
			using (IDataReader reader = dbcmd.ExecuteReader())
			{
					return reader;
			}


		}
  }
}
