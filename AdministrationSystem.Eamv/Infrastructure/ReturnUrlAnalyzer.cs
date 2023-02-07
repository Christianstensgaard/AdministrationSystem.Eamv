using Microsoft.IdentityModel.Tokens;
using System;

namespace AdministrationSystem.Eamv.Infrastructure
{
    public class returnUrlAnalyzer
    {

        string returnUrl;
        string[] data;
        int startindex = 0;

        const char StartSplit  = '/';
        const char SecondSplit = '&';
        const char ThirdSplit  = '=';

        /// <summary>
        /// Dictionary: 
        /// </summary>
        public Dictionary<string, string> ValueSet { get; private set; }
        /// <summary>
        /// Take the return URL, 
        /// </summary>
        /// <param name="returnUrl"></param>
        public returnUrlAnalyzer(string returnUrl)
        {
            this.returnUrl = returnUrl;
        }
        /// <summary>
        /// Checking the path type, 
        /// </summary>
        /// <returns>If Preview or not</returns>
        public bool CheckPath()
        {
            if (returnUrl.Split(StartSplit)[0].Equals("Preview"))
            {
                startindex = 1;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Taking the return URL, and analyzing. 
        /// </summary>
        public void Analyze()
        {
                                                
            data = returnUrl.Split(StartSplit); 

            ValueSet = new Dictionary<string, string>();
            string[] buffer = data[startindex].Split(SecondSplit);

            for (int i = 0; i < buffer.Length; i++)
            {
                string[] bufferdata = buffer[i].Split(ThirdSplit);
                ValueSet[bufferdata[0]] = bufferdata[1];
            }
        }
        /// <summary>
        /// Getting the value of x in a dictionary. 
        /// </summary>
        /// <param name="key">key - from the dictionary </param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            string a = "";
            ValueSet.TryGetValue(key, out a);
            ValueSet.Remove(key);

            if (a == null)
                a = "";

            return a;
        }
    }
}
