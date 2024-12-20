using System;
using System.Collections.Generic;
using Modules.UnityHttpClient.CommonSerializers.Json;
using Modules.UnityHttpClient.CommonSerializers.PlainText;
using Modules.UnityHttpClient.Interfaces;

namespace Modules.UnityHttpClient
{
    public class HttpRequestBuilder<TRequest, TResponse>
    {
        private string _method;
        private string _url;
        private readonly Dictionary<string, string> _headers = new();
        private TRequest _requestData;
        private IBodySerializer _bodySerializer;
        private bool _useJson;
        private bool _usePlainText;
        private byte[] _rawPayload;
        private string _rawContentType;
        private IHttpResponseMapper<TResponse> _responseMapper;
        private int? _timeout;
        private int _retryCount;
        private int _retryDelayMilliseconds;

        public HttpRequestBuilder<TRequest, TResponse> SetMethod(string method)
        {
            _method = method;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> SetUrl(string url)
        {
            _url = url;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> AddHeader(string name, string value)
        {
            _headers[name] = value;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> SetRequestData(TRequest data)
        {
            _requestData = data;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> UseJson()
        {
            _useJson = true;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> UsePlainText()
        {
            _usePlainText = true;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> UseCustomRequestSerializer(IBodySerializer serializer)
        {
            _bodySerializer = serializer;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> SetRawPayload(string payload, string contentType)
        {
            _rawPayload = System.Text.Encoding.UTF8.GetBytes(payload ?? string.Empty);
            _rawContentType = contentType;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> UseCustomResponseMapper(IHttpResponseMapper<TResponse> mapper)
        {
            _responseMapper = mapper;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> SetTimeout(int timeoutSeconds)
        {
            _timeout = timeoutSeconds;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> SetRetryCount(int retryCount)
        {
            _retryCount = retryCount;
            return this;
        }

        public HttpRequestBuilder<TRequest, TResponse> SetRetryDelay(int milliseconds)
        {
            _retryDelayMilliseconds = milliseconds;
            return this;
        }

        public HttpRequestConfiguration<TResponse> Build()
        {
            EnsureSingleFormatSelected();
            ConfigureDefaultSerializersIfNeeded();
            ValidateResponseMapperAvailability();

            var (payload, contentType) = CreateFinalPayloadAndContentType();

            if (!string.IsNullOrEmpty(contentType))
            {
                _headers.TryAdd("Content-Type", contentType);
            }

            var httpRequest = new HttpRequest(
                method: _method,
                url: _url,
                headers: new Dictionary<string, string>(_headers),
                payload: payload,
                contentType: contentType,
                timeout: _timeout
            );

            return new HttpRequestConfiguration<TResponse>(
                httpRequest: httpRequest,
                responseMapper: _responseMapper,
                retryCount: _retryCount,
                retryDelayMilliseconds: _retryDelayMilliseconds
            );
        }

        private void EnsureSingleFormatSelected()
        {
            if (_useJson && _usePlainText)
            {
                throw new InvalidOperationException("Cannot use both JSON and PlainText. Please choose one format.");
            }
        }

        private void ConfigureDefaultSerializersIfNeeded()
        {
            if (_useJson && _bodySerializer == null)
            {
                _bodySerializer = new JsonBodySerializer();
            }

            if (_useJson && _responseMapper == null)
            {
                _responseMapper = new JsonResponseMapper<TResponse>();
            }

            if (_usePlainText && _bodySerializer == null)
            {
                _bodySerializer = new TextBodySerializer();
            }

            if (_usePlainText && _responseMapper == null)
            {
                _responseMapper = new TextResponseMapper<TResponse>();
            }
        }

        private void ValidateResponseMapperAvailability()
        {
            if (_responseMapper == null)
            {
                throw new InvalidOperationException(
                    "No response mapper provided. Call UseJson(), UsePlainText(), or UseCustomResponseMapper()."
                );
            }
        }

        private (byte[] payload, string contentType) CreateFinalPayloadAndContentType()
        {
            if (_rawPayload != null)
            {
                return (_rawPayload, _rawContentType);
            }

            if (_requestData != null && _bodySerializer != null)
            {
                var payload = _bodySerializer.Serialize(_requestData);
                var contentType = _bodySerializer.ContentType;
                return (payload, contentType);
            }

            return (null, null);
        }
    }
}