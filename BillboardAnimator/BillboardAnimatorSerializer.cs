using BillboardAnimator.Managers;
using BillboardAnimator.Utils;
using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BillboardAnimator
{
    public class BillboardAnimatorSerializer : SerializableDataExtensionBase
    {
        private readonly string propDictKey = "BillboardAnimatorProps";
        private readonly string buildingDictKey = "BillboardAnimatorBuildings";

        public override void OnSaveData()
        {
            LoggerUtils.Log("Saving Screen Objects");

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream propDictStream = new MemoryStream();
            MemoryStream buildingDictStream = new MemoryStream();

            try
            {
                ScreenObj[] propDict = ScreenManager.instance.saveScreen(true);
                ScreenObj[] buildingDict = ScreenManager.instance.saveScreen(false);

                if( propDict != null)
                {
                    binaryFormatter.Serialize(propDictStream, propDict);
                    serializableDataManager.SaveData(propDictKey, propDictStream.ToArray());
                    LoggerUtils.Log("Prop screen objs have been saved!");
                }
                else
                {
                    LoggerUtils.LogWarning("Couldn't save props, as the array is null!");
                }


                if (buildingDict != null)
                {
                    binaryFormatter.Serialize(buildingDictStream, buildingDict);
                    serializableDataManager.SaveData(buildingDictKey, buildingDictStream.ToArray());
                    LoggerUtils.Log("building screen objs have been saved!");
                }
                else
                {
                    LoggerUtils.LogWarning("Couldn't save buildings, as the array is null!");
                }


            }
            catch (Exception ex)
            {
                LoggerUtils.LogException(ex);
            }
            finally
            {
                buildingDictStream.Close();
                propDictStream.Close();
            }
        }

        public override void OnLoadData()
        {
            LoggerUtils.Log("Loading routes");

            byte[] loadPropDict = serializableDataManager.LoadData(propDictKey);
            byte[] loadBuildingDict = serializableDataManager.LoadData(buildingDictKey);

            if (loadPropDict != null)
            {
                MemoryStream propDictStream = new MemoryStream();

                propDictStream.Write(loadPropDict, 0, loadPropDict.Length);
                propDictStream.Position = 0;

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                try
                {
                    ScreenObj[] propArray = binaryFormatter.Deserialize(propDictStream) as ScreenObj[];

                    if (propArray != null)
                    {
                        ScreenManager.instance.loadScreen(true, propArray);
                    }
                    else
                    {
                        LoggerUtils.LogWarning("Couldn't load props, as the array is null!");
                    }
                }
                catch (Exception ex)
                {
                    LoggerUtils.LogException(ex);

                }
                finally
                {
                    propDictStream.Close();
                }
            }
            else
            {
                LoggerUtils.LogWarning("Found no data to load");
            }

            if (loadBuildingDict != null)
            {
                MemoryStream buildingPropDict = new MemoryStream();

                buildingPropDict.Write(loadBuildingDict, 0, loadBuildingDict.Length);
                buildingPropDict.Position = 0;

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                try
                {
                    ScreenObj[] buildingArray = binaryFormatter.Deserialize(buildingPropDict) as ScreenObj[];

                    if (buildingArray != null)
                    {
                        ScreenManager.instance.loadScreen(false, buildingArray);
                    }
                    else
                    {
                        LoggerUtils.LogWarning("Couldn't load building, as the array is null!");
                    }
                }
                catch (Exception ex)
                {
                    LoggerUtils.LogException(ex);

                }
                finally
                {
                    buildingPropDict.Close();
                }
            }
            else
            {
                LoggerUtils.LogWarning("Found no data to load");
            }
            
        }

    }
}
