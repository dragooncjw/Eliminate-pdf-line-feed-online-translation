using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace lwtool
{
    class HandWhiteTarans
    {
        /// <summary> 
        /// 使用WebRequest获取Google翻译后的内容 
        /// </summary> 
        /// <param name="strTranslateString">需要翻译的内容</param> 
        /// <param name="strRequestLanguage">原文语种</param> 
        /// <param name="strResultLanguage">译文语种</param> 
        /// <returns></returns> 

        private string GetGoogleTrans(string strTransString, string strRequesLanguage, string strResultLanguage)
        {
            WebRequest request = HttpWebRequest.Create(
                "http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=" + strTransString + "&langpair=" + strRequesLanguage + "%7C" + strResultLanguage
                );
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

        public string MultiLanguageTranslate(string strTranslateString, string
         strRequestLanguage, string strResultLanguage)
        {
            try
            {
                if (!string.IsNullOrEmpty(strTranslateString))
                {
                    TranslateString transtring = (TranslateString)Newtonsoft.Json.JsonConvert.DeserializeObject(GetGoogleTrans(strTranslateString, strRequestLanguage, strResultLanguage), typeof(TranslateString));
                    if (transtring.responseStatus == 200)
                        return transtring.responseData.translatedText;
                    else
                        return "There was an error.";
                }
                else
                {
                    return strTranslateString;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// 翻译
        /// 默认地将英文翻译成简体中文
        /// </summary>
        /// <param name="TranslateString">需要翻译的内容</param>
        /// <returns></returns>   
        public string TranslateEnglishToChinese(string strTranslateString)
        {
            return MultiLanguageTranslate(strTranslateString, "en", "zh-CN");
        }

        /// <summary>
        /// 翻译
        /// 默认地将简体中文翻译成英文
        /// </summary>
        /// <param name="TranslateString">需要翻译的内容</param>
        /// <returns></returns>
        public string TranslateChineseToEnglish(string strTranslateString)
        {
            return MultiLanguageTranslate(strTranslateString, "zh-CN", "en");
        }

    }


    /// <summary> 
    /// 译文的反系列化类 
    /// </summary> 
    public class TranslateString
    {
        private TranslatedText responsedata;
        public TranslatedText responseData
        {
            get { return responsedata; }
            set { responsedata = value; }
        }
        private string responsedetails;
        public string responseDetails
        {
            get { return responsedetails; }
            set { responsedetails = value; }
        }
        private int responsestatus;
        public int responseStatus
        {
            get { return responsestatus; }
            set { responsestatus = value; }
        }
        /**/
        /// <summary> 
        /// 译文内容 
        /// </summary> 
        public class TranslatedText
        {
            private string translatedtext;
            public string translatedText
            {
                get { return translatedtext; }
                set { translatedtext = value; }
            }
        }
    }
}
