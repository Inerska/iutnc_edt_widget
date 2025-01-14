﻿using Microsoft.Extensions.Configuration;
using RestSharp;

namespace ULApi.BusinessLayer.Services;

/// <summary>
/// Concrete implementation of Graph strategy business layer fetcher service.
/// </summary>
/// <typeparam name="TItem">Resulted type after data fetching.</typeparam>
public class GraphBusinessFetcherService<TItem>
    : IBusinessFetcherService<TItem>
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphBusinessFetcherService{TItem}"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public GraphBusinessFetcherService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Asynchronous fetching data of TItem, sending query through POST method.
    /// </summary>
    /// <param name="query">Query to send through POST method.</param>
    /// <returns>Response of type TItem.</returns>
    public async Task<TItem> FetchAsync(string query)
    {
        // ReSharper disable once SettingNotFoundInConfiguration
        // User secret Api endpoint base.
        var apiUrl = _configuration["Api:Endpoint_Base"];
        ArgumentNullException.ThrowIfNull(apiUrl);

        var client = new RestClient();
        var request = new RestRequest {
            Method = Method.GET,
            Resource = apiUrl
        };

        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(new {
            query
        });
        var response = await client.PostAsync<TItem>(request);
        return response;
    }
}