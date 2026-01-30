using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IIstudyWSClient
{
    public class ApiClient<T>
    {
        HttpClient httpClient = IIStudyHttpClient.Insatnce;
        UriBuilder uriBuilder = new UriBuilder();
        public string Scheme
        {
            set {
                this.uriBuilder.Scheme = value;
            }
            
        }

        public int Port
        {
            set
            {
                this.uriBuilder.Port = value;
            }
        }
        public string Host
        {
            set
            {
                this.uriBuilder.Host = value;
            }
        }
        public string Path
        {
            set
            {
                this.uriBuilder.Path = value;
            }
        }
        public void AddParameter(string key, object value)
        {
            if(this.uriBuilder.Query == string.Empty)
            {
                this.uriBuilder.Query += "?";
            }
            else
                this.uriBuilder.Query += "&";

            this.uriBuilder.Query += $"{key}={Convert.ToString(value)}";
        }

        public async Task<T> GetAsync()
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                using (HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest)){

                    //await Console.Out.WriteLineAsync(httpResponse.IsSuccessStatusCode.ToString());
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string result = await httpResponse.Content.ReadAsStringAsync();
                        if (string.IsNullOrWhiteSpace(result))
                        {
                            return default(T);
                        }
                        await Console.Out.WriteLineAsync( result);
                        //string result = httpResponse.Content.ToString();
                        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                        jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                        T model = JsonSerializer.Deserialize<T>(result, jsonSerializerOptions);
                        //PropertyInfo pro = model.GetType().GetProperty("BookID");
                        //await Console.Out.WriteLineAsync($"model: {(string)pro.GetValue(model, null)}");
                        return model;
                    }
                    return default(T);
                }
            };

        }

        public async Task<ApiFileResultModel> GetAsyncFile()
        {
            
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                ApiFileResultModel resultModel = new ApiFileResultModel();
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                using (HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest))
                {

                    //await Console.Out.WriteLineAsync(httpResponse.IsSuccessStatusCode.ToString());
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        byte[] file = await httpResponse.Content.ReadAsByteArrayAsync();
                        string contentType = httpResponse.Content.Headers.ContentType?.ToString() ?? "image/jpeg";
                        //string result = httpResponse.Content.ToString();
                        //PropertyInfo pro = model.GetType().GetProperty("BookID");
                        //await Console.Out.WriteLineAsync($"model: {(string)pro.GetValue(model, null)}");
                        resultModel.Bytes = file;
                        resultModel.ContentType = contentType;


                        return resultModel;
                    }
                    return null;
                }
            }
            ;

        }

        public async Task<bool> PostAsync(T model, List<(Stream stream, string fileName)> files = null, bool return_obj = false)
        {
            using(HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormData = new MultipartFormDataContent();
                string json = JsonSerializer.Serialize(model);

                StringContent content  = new StringContent(json, Encoding.UTF8, "application/json");

                multipartFormData.Add(content, "model");
                //If error raises, check files is null and than loop if not.
                if (files != null)
                {
                    foreach (var (stream, fileName) in files)
                    {
                        StreamContent streamContent = new StreamContent(stream);
                        multipartFormData.Add(streamContent, "file", fileName);
                    }
                    httpRequest.Content = multipartFormData;
                }
                else
                {
                    httpRequest.Content = content;
                }
                using(HttpResponseMessage response = await httpClient.SendAsync(httpRequest))
                {
                    return response.IsSuccessStatusCode;
                    
                }
            }
        }

        public async Task<ApiResultModel<TResponse>> PostAsyncRet<T,TResponse>(T model, List<Stream> files = null)
        {
            using (HttpRequestMessage httpRequest = new HttpRequestMessage())
            {
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = this.uriBuilder.Uri;
                MultipartFormDataContent multipartFormData = new MultipartFormDataContent();
                string json = JsonSerializer.Serialize(model);

                StringContent content = new StringContent(json,Encoding.UTF8,"application/json");

                multipartFormData.Add(content, "model");
                //If error raises, check files is null and than loop if not.
                if (files != null) { 
                foreach (Stream file in files)
                {
                    StreamContent streamContent = new StreamContent(file);
                    multipartFormData.Add(streamContent, "file", "file");
                }
                httpRequest.Content = multipartFormData;
                }
                else
                {
                    httpRequest.Content = content;
                }
                using (HttpResponseMessage response = await httpClient.SendAsync(httpRequest))
                {
                    ApiResultModel<TResponse> apiResult = new ApiResultModel<TResponse>();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("here");
                        string result = await response.Content.ReadAsStringAsync();
                        await Console.Out.WriteLineAsync(result);
                        if (string.IsNullOrWhiteSpace(result))
                        {
                            apiResult.Success = false;
                            return apiResult;
                        }
                        await Console.Out.WriteLineAsync(result);
                        //string result = httpResponse.Content.ToString();
                        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
                        jsonSerializerOptions.PropertyNameCaseInsensitive = true;
                        TResponse modelRet = JsonSerializer.Deserialize<TResponse>(result, jsonSerializerOptions);
                        apiResult.Data = modelRet;
                        apiResult.StatusCode = response.StatusCode;
                        apiResult.Success = response.IsSuccessStatusCode;
                        //PropertyInfo pro = model.GetType().GetProperty("BookID");
                        //await Console.Out.WriteLineAsync($"model: {(string)pro.GetValue(model, null)}");
                        return apiResult;
                    }
                    apiResult.Success = false;
                    apiResult.StatusCode = response.StatusCode;
                    return apiResult;

                }
            }
        }

        //public async Task<ApiResultModel<TResponse>> PostAsyncRet<T, TResponse>(T model)
        //{
        //    using (HttpRequestMessage httpRequest = new HttpRequestMessage())
        //    {
        //        httpRequest.Method = HttpMethod.Post;
        //        httpRequest.RequestUri = this.uriBuilder.Uri;
        //        string json = JsonSerializer.Serialize(model);
        //        httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        //        using (HttpResponseMessage response = await httpClient.SendAsync(httpRequest))
        //        {
        //            ApiResultModel<TResponse> apiResult = new ApiResultModel<TResponse>();

        //            if (response.IsSuccessStatusCode)
        //            {
        //                Console.WriteLine("here");
        //                string result = await response.Content.ReadAsStringAsync();
        //                await Console.Out.WriteLineAsync(result);
        //                //string result = httpResponse.Content.ToString();
        //                JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
        //                jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        //                TResponse modelRet = JsonSerializer.Deserialize<TResponse>(result, jsonSerializerOptions);
        //                apiResult.Data = modelRet;
        //                apiResult.StatusCode = response.StatusCode;
        //                apiResult.Success = response.IsSuccessStatusCode;
        //                //PropertyInfo pro = model.GetType().GetProperty("BookID");
        //                //await Console.Out.WriteLineAsync($"model: {(string)pro.GetValue(model, null)}");
        //                return apiResult;
        //            }
        //            apiResult.Success = false;
        //            apiResult.StatusCode = response.StatusCode;
        //            return apiResult;

        //        }
        //    }
        //}
        public void print_url()
        {
            Console.WriteLine(this.uriBuilder.ToString());
        }
        public string GetUrl()
        {
            return this.uriBuilder.ToString();
        }
    }
}
