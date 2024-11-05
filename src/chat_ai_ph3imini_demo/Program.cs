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
