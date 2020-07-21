using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace XLocalizer.Translate.SystranTranslate
{
    /// <summary>
    /// SYSTRAN.io translation service
    /// </summary>
    public class SystranTranslateService : ITranslator
    {
        /// <summary>
        /// Service name
        /// </summary>
        public string ServiceName => "SYSTRAN.io Translate";

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly string _rapidApiKey;

        /// <summary>
        /// Initialize SYSTRAN.io translate service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public SystranTranslateService(HttpClient httpClient, IConfiguration configuration, ILogger<SystranTranslateService> logger)
        {
            _httpClient = httpClient;
            _rapidApiKey = configuration["XLocalizer.TranslationServices:RapidApiKey"];
            _logger = logger;
        }

        /// <summary>
        /// Run async translation task
        /// </summary>
        /// <param name="source">Source language e.g. en</param>
        /// <param name="target">Target language e.g. tr</param>
        /// <param name="text">Text to be translated</param>
        /// <param name="format">Text format: html or text</param>
        /// <returns><see cref="TranslationResult"/></returns>
        public async Task<TranslationResult> TranslateAsync(string source, string target, string text, string format)
        {
            if (string.IsNullOrWhiteSpace(_rapidApiKey))
            {
                throw new NullReferenceException(nameof(_rapidApiKey));
            }

            try
            {

                _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _rapidApiKey);
                _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "systran-systran-platform-for-language-processing-v1.p.rapidapi.com");

                var response = await _httpClient.GetAsync($"https://systran-systran-platform-for-language-processing-v1.p.rapidapi.com/translation/text/translate?source={source}&target={target}&input={text}");
                _logger.LogInformation($"Response: {ServiceName} - {response.StatusCode}");
                /*
                 * Sample response :
                 * {
                 *     "outputs":[
                 *         { 
                 *             "output":"Geri",
                 *             "stats": { "elapsed_time":1057,"nb_characters":4,"nb_tokens":1,"nb_tus":1,"nb_tus_failed":0 } 
                 *         }
                 *     ]
                 * }
                 */
                var responseContent = await response.Content.ReadAsStringAsync();

                var responseDto = JsonConvert.DeserializeObject<SystranTranslateResult>(responseContent);

                return new TranslationResult
                {
                    Text = responseDto.Outputs[0].Output,
                    StatusCode = response.StatusCode,
                    Target = target,
                    Source = source
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Error {ServiceName} - {e.Message}");
            }

            return new TranslationResult
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Text = text,
                Target = target,
                Source = source
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        public bool TryTranslate(string source, string target, string text, out string translation)
        {
            var trans = Task.Run(() => TranslateAsync(source, target, text, "text")).GetAwaiter().GetResult();

            if (trans.StatusCode == HttpStatusCode.OK)
            {
                translation = trans.Text;
                return true;
            }

            translation = text;
            return false;
        }
    }
}
