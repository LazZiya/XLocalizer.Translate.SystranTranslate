using Newtonsoft.Json;

namespace XLocalizer.Translate.SystranTranslate
{
    /// <summary>
    /// SYSTRAN.io translate result
    /// </summary>
    public class SystranTranslateResult
    {
        /// <summary>
        /// Outputs
        /// </summary>
        [JsonProperty("outputs")]
        public SystranResultOutput[] Outputs { get; set; }
    }

    /// <summary>
    /// translate result output
    /// </summary>
    public class SystranResultOutput
    {
        /// <summary>
        /// Output translated text
        /// </summary>
        [JsonProperty("output")]
        public string Output { get; set; }
    }
}
