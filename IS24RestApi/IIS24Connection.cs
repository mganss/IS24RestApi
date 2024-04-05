﻿using System;
using System.Threading.Tasks;
using RestSharp;

namespace IS24RestApi
{
    /// <summary>
    /// Describes the connection used for preparing and executing the rest requests to IS24
    /// </summary>
    public interface IIS24Connection
    {
        /// <summary>
        /// The common URL prefix of all resources (e.g. "http://rest.sandbox-immobilienscout24.de/restapi/api").
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

        /// <summary>
        /// The factory of IRestClient objects that is used to communicate with the service.
        /// Used mainly for testing purposes.
        /// </summary>
        Func<string, ConfigureRestClient, IRestClient> RestClientFactory { get; set; }

        /// <summary>
        /// Creates a basic <see cref="IRestRequest"/> instance for the given resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        RestRequest CreateRequest(string resource, Method method = Method.Get);

        /// <summary>
        /// Performs an API request as an asynchronous task.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="request">The request object.</param>
        /// <param name="baseUrl">The suffix added to <see cref="IS24Connection.BaseUrlPrefix"/> to obtain the request URL.</param>
        /// <returns>The task representing the request.</returns>
        Task<T> ExecuteAsync<T>(RestRequest request, string baseUrl = null) where T : new();
    }
}