XLocalizer.TranslationServices.SystranTranslate

Instructions to use this package :

- This package requires Rapid API Key, must be obtained from https://rapidapi.com/systran/api/systran-io-translation-and-nlp
- Add the API key to user secrets :

````
{
  "XLocalizer.TranslationServices": {
    "RapidApiKey": "xxx-rapid-api-key-xxx"
  }
}
````

- Register in startup:
````
services.AddHttpClient<ITranslationService, SystranTranslateService>();
````

Repository: https://github.com/LazZiya/TranslationServices