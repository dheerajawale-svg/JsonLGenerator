# jsonl-generator-dotnet
 Generates jsonl file that can be fed to openai finetuning model


Converts json to jsonl format that includes below format:

```json
{
    "messages": [
        {
            "role": "system",
            "content": "You are a XYZ"
        },
        {
            "role": "user",
            "content": "Are there times when you ABC"
        },
        {
            "role": "assistant",
            "content": "yes, I have more difficulty TWX"
        },
        #....etc...
    ]
}
```
