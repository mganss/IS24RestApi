﻿using System.Threading.Tasks;
using RestSharp;

namespace IS24RestApi
{
    public interface IIS24Client
    {
        /// <summary>
        /// The URL prefix including the user part
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// The common URL prefix of all resources (e.g. "http://rest.sandbox-immobilienscout24.de/restapi/api/offer/v1.0").
        /// </summary>
        string BaseUrlPrefix { get; set; }

        /// <summary>
        /// The OAuth ConsumerSecret
        /// </summary>
        string ConsumerSecret { get; set; }

        /// <summary>
        /// The OAuth ConsumerKey
        /// </summary>
        string ConsumerKey { get; set; }

        /// <summary>
        /// The OAuth AccessToken
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// The OAuth AccessTokenSecret
        /// </summary>
        string AccessTokenSecret { get; set; }

        RestRequest Request(string resource, Method method = Method.GET);

        /// <summary>
        /// Performs an API request as an asynchronous task.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="baseUrl">The suffix added to <see cref="IS24Client.BaseUrlPrefix"/> to obtain the request URL. If null, <see cref="IS24Client.BaseUrl"/> will be used.</param>
        /// <returns>The task representing the request.</returns>
        Task<T> ExecuteAsync<T>(RestRequest request, string baseUrl = null) where T : new();
    }
}