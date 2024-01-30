﻿// Copyright (c) Microsoft. All rights reserved.

using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Http;
using Microsoft.SemanticKernel.Services;

namespace Microsoft.SemanticKernel.Connectors.GoogleVertexAI;

/// <summary>
/// Represents a chat completion service using Google AI Gemini API.
/// </summary>
public sealed class GoogleAIGeminiChatCompletionService : GeminiChatCompletionServiceBase
{
    /// <summary>
    /// Initializes a new instance of the GeminiChatCompletionService class.
    /// </summary>
    /// <param name="model">The Gemini model for the chat completion service.</param>
    /// <param name="apiKey">The API key for authentication with the Gemini client.</param>
    /// <param name="httpClient">Optional HTTP client to be used for communication with the Gemini API.</param>
    /// <param name="loggerFactory">Optional logger factory to be used for logging.</param>
    public GoogleAIGeminiChatCompletionService(
        string model,
        string apiKey,
        HttpClient? httpClient = null,
        ILoggerFactory? loggerFactory = null)
    {
        Verify.NotNullOrWhiteSpace(model);
        Verify.NotNullOrWhiteSpace(apiKey);

        this.ChatCompletionClient = new GeminiChatCompletionClient(
#pragma warning disable CA2000
            httpClient: HttpClientProvider.GetHttpClient(httpClient),
#pragma warning restore CA2000
            modelId: model,
            httpRequestFactory: new GoogleAIGeminiHttpRequestFactory(),
            endpointProvider: new GoogleAIGeminiEndpointProvider(apiKey),
            logger: loggerFactory?.CreateLogger(typeof(GoogleAIGeminiChatCompletionService)));
        this.TextGenerationClient = new GeminiTextGenerationClient(this.ChatCompletionClient);
        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, model);
    }

    internal GoogleAIGeminiChatCompletionService(
        IGeminiChatCompletionClient chatCompletionClient,
        IGeminiTextGenerationClient textGenerationClient,
        string modelId)
    {
        this.ChatCompletionClient = chatCompletionClient;
        this.TextGenerationClient = textGenerationClient;
        this.AttributesInternal.Add(AIServiceExtensions.ModelIdKey, modelId);
    }
}