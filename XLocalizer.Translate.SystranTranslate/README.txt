XLocalizer.Translate.SystranTranslate

Instructions to use this package :

- This package requires Rapid API Key, must be obtained from https://rapidapi.com/systran/api/systran-io-translation-and-nlp
- Add the API key to user secrets :

````
{
  "XLocalizer.Translate": {
    "RapidApiKey": "xxx-rapid-api-key-xxx"
  }
}
````

- Register in startup:
````
services.AddHttpClient<ITranslator, SystranTranslateService>();
````

Repository: https://github.com/LazZiya/XLocalizer.Translate.SystranTranslate