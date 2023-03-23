using ClpCash.DTO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ClpCash.Repository
{
    public class Util
    {
        public static Data DesserializarXml(string XMLResponse)
        {
            if (string.IsNullOrEmpty(XMLResponse)) return default(Data);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XMLResponse);
            XmlNode myNode = null;
            myNode = doc.DocumentElement;
            Data obj = null;

            try
            {
                XmlSerializer xmlSerializer = null;
                MemoryStream msStream = null;

                xmlSerializer = new XmlSerializer(typeof(Data));
                msStream = new MemoryStream(Encoding.UTF8.GetBytes(XMLResponse));

                obj = (Data)xmlSerializer.Deserialize(msStream);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Erro ao desserializar arquivo xml!", e);
            }
            return obj;
        }

        public static string TrataString(string desLogAcao)
        {
            string novolog = "";

            if (desLogAcao != "")
            {
                novolog = desLogAcao.Substring(desLogAcao.LastIndexOf("<?xml"));
            }
            else
            {
                novolog = "Não houve retorno de log na consulta/ação relacionado ao box";
                //novolog = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\r\n<data>\r\n<var><Resultado>Não houve retorno de log na consulta/ação ao box! (Método: InserirLog)</Resultado></var>\r\n</data>\r\n";
            }

            return novolog;
        }

        public static string CapturarStatusAtual(string xml, CLPCash clp)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml.ToString());

                XmlNodeList xnList = xmlDoc.GetElementsByTagName("var");

                string flg_status;

                for (int i = 0; i < xnList.Count; i++)
                {
                    if (xnList[i]["name"].InnerText == string.Concat(clp.Des_cofre, '.', clp.Nom_tecnico).ToString())
                    {
                        flg_status = xnList[i]["value"].InnerText;
                        return flg_status;
                    }
                    else
                    {
                        return "1";
                    }
                }
                return "1";
            }
            catch(Exception e)
            {
                return "";
            }

        }

    }
}
