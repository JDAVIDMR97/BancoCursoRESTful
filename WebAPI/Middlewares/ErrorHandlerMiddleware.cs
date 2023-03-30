﻿using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var respondeModel = new Response<string>() { Succeeded = false, Message = error?.Message };
                switch (error)
                {
                    case Application.Exceptions.ApiException e:
                        //custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case Application.Exceptions.ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        respondeModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        //not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                        default:
                        //unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        break;
                }

                var result = JsonSerializer.Serialize(respondeModel);
                await response.WriteAsync(result);  
            }
        }
    }
}
