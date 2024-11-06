# chat_ai_phi3mini_demo
Based on the phi3:mini large model, run in Ollama and connect to chat with the AI.

## Phi open models

**Phi open models**是功能强大的小语言模型(SLMs)，具有开创性的低成本和低延迟。包含：Phi--3-version、Phi--3-medium、Phi--3-small、Phi--3-mini，具有可以运行在任何地方 CPUs and  GPUs;cloud and on device，快速推理与响应，长文本。

### 不同版本有什么区别

* Phi-1: For Python coding; 用于python编码

* Phi-1.5: For reasoning and understanding; 用于推理和理解

* Phi-2: For language comprehension; 用于语言理解

* Phi-3: For language understanding and reasoning tasks; 用于语言理解和推理任务，在编码基准方面也表现良好

### 可以用于生产环境吗？

可以，从phi-3开始的所有模型都是为生产用例设计的。

### 可以在那里获取Phi

可以从 Azure AI Studio model catalog, Hugging Face, and Ollama.

### Phi训练截至时间

2023年10月

## Ollama

[Ollama](https://ollama.com/)是启动并运行大型语言模型的工具，运行 Llama 3.2，Phi 3，Mistral，Gemma 2和其他模型，自定义并创建你自己的。

### 1. 下载安装Ollama

```bash
# 验证是否安装成功
ollama -v
# 拉取phi3:mini模型，等待下载完成
ollama pull phi3:mini
# 运行模型
ollama run phi3:mini

// Ollama 退出 Ctrl + d
```



## 创建.Net程序

### 1. 创建控制台程序

要求**.NET 8.0**以上

安装nuget包

```bash
dotnet add package Microsoft.SemanticKernel
dotnet add package Microsoft.SemanticKernel.Connectors.Ollama // 预览版
```



### 2. 编写交互代码

```csharp
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
namespace chat_ai_ph3imini_demo
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
#pragma warning disable SKEXP0070
            Kernel kernel = Kernel.CreateBuilder()
                .AddOllamaChatCompletion(
                    modelId: "phi3:mini",
                    endpoint: new Uri("http://localhost:11434") // Ollama默认运行端口 11434
                ).Build();

            var aiChatService = kernel.GetRequiredService<IChatCompletionService>();
            var chatHistory = new ChatHistory();

            while (true)
            {
                Console.WriteLine("你的提问:");
                var userPrompt = Console.ReadLine();
                chatHistory.Add(new ChatMessageContent(AuthorRole.User, userPrompt));

                Console.WriteLine("AI回答:");
                var response = "";
                await foreach (var item in aiChatService.GetStreamingChatMessageContentsAsync(chatHistory))
                {
                    Console.Write(item.Content);
                    response += item.Content;
                }
                chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, response));
                Console.WriteLine();
            }
        }
    }
}
```

- Creates a `Kernel` object and uses it to retrieve a chat completion service.
- Creates a `ChatHistory` object to store the messages between the user and the AI model.
- Retrieves a prompt from the user and stores it in the `ChatHistory`.
- Sends the chat data to the AI model to generate a response.

![char_ai_demo](/img/chat_ai_demo.png)



## 学习参考：

1. [Quickstart - Connect to and chat with a local AI using .NET and Semantic Kernel - .NET | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/quickstart-local-ai)