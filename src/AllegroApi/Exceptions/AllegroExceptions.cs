using System.Net;

namespace AllegroApi.Exceptions;

/// <summary>
/// Base exception for all Allegro API errors
/// </summary>
public class AllegroApiException : Exception
{
    /// <summary>
    /// HTTP status code of the error response.
    /// </summary>
    public HttpStatusCode? StatusCode { get; set; }
    
    /// <summary>
    /// Allegro-specific error code.
    /// </summary>
    public string? ErrorCode { get; set; }
    
    /// <summary>
    /// Additional error metadata.
    /// </summary>
    public Dictionary<string, object>? ErrorMetadata { get; set; }

    /// <summary>
    /// Initializes a new instance of AllegroApiException.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AllegroApiException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of AllegroApiException with inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public AllegroApiException(string message, Exception innerException) 
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of AllegroApiException with status code.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="errorCode">Allegro-specific error code.</param>
    public AllegroApiException(string message, HttpStatusCode statusCode, string? errorCode = null) 
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}

/// <summary>
/// Exception thrown when authentication fails
/// </summary>
public class AllegroAuthenticationException : AllegroApiException
{
    /// <summary>
    /// Initializes a new instance of AllegroAuthenticationException.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AllegroAuthenticationException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of AllegroAuthenticationException with inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public AllegroAuthenticationException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when authorization fails (403)
/// </summary>
public class AllegroAuthorizationException : AllegroApiException
{
    /// <summary>
    /// Initializes a new instance of AllegroAuthorizationException.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AllegroAuthorizationException(string message) 
        : base(message, HttpStatusCode.Forbidden) { }

    /// <summary>
    /// Initializes a new instance of AllegroAuthorizationException with error code.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">Allegro-specific error code.</param>
    public AllegroAuthorizationException(string message, string errorCode) 
        : base(message, HttpStatusCode.Forbidden, errorCode) { }
}

/// <summary>
/// Exception thrown when a resource is not found (404)
/// </summary>
public class AllegroNotFoundException : AllegroApiException
{
    /// <summary>
    /// Type of resource that was not found.
    /// </summary>
    public string? ResourceType { get; set; }
    
    /// <summary>
    /// Identifier of the resource that was not found.
    /// </summary>
    public string? ResourceId { get; set; }

    /// <summary>
    /// Initializes a new instance of AllegroNotFoundException.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AllegroNotFoundException(string message) 
        : base(message, HttpStatusCode.NotFound) { }

    /// <summary>
    /// Initializes a new instance of AllegroNotFoundException with resource details.
    /// </summary>
    /// <param name="resourceType">Type of resource.</param>
    /// <param name="resourceId">Resource identifier.</param>
    public AllegroNotFoundException(string resourceType, string resourceId) 
        : base($"{resourceType} with ID '{resourceId}' not found", HttpStatusCode.NotFound)
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }
}

/// <summary>
/// Exception thrown when request validation fails (400)
/// </summary>
public class AllegroBadRequestException : AllegroApiException
{
    /// <summary>
    /// List of validation errors.
    /// </summary>
    public List<Models.Common.Error> ValidationErrors { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of AllegroBadRequestException.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AllegroBadRequestException(string message) 
        : base(message, HttpStatusCode.BadRequest) { }

    /// <summary>
    /// Initializes a new instance of AllegroBadRequestException with validation errors.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">List of validation errors.</param>
    public AllegroBadRequestException(string message, List<Models.Common.Error> errors) 
        : base(message, HttpStatusCode.BadRequest)
    {
        ValidationErrors = errors;
    }
}

/// <summary>
/// Exception thrown when business logic validation fails (422)
/// </summary>
public class AllegroUnprocessableEntityException : AllegroApiException
{
    /// <summary>
    /// List of validation errors.
    /// </summary>
    public List<Models.Common.Error> ValidationErrors { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of AllegroUnprocessableEntityException.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AllegroUnprocessableEntityException(string message) 
        : base(message, (HttpStatusCode)422) { }

    /// <summary>
    /// Initializes a new instance of AllegroUnprocessableEntityException with validation errors.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">List of validation errors.</param>
    public AllegroUnprocessableEntityException(string message, List<Models.Common.Error> errors) 
        : base(message, (HttpStatusCode)422)
    {
        ValidationErrors = errors;
    }
}

/// <summary>
/// Exception thrown when there's a conflict (409)
/// </summary>
public class AllegroConflictException : AllegroApiException
{
    public AllegroConflictException(string message) 
        : base(message, HttpStatusCode.Conflict) { }

    public AllegroConflictException(string message, string errorCode) 
        : base(message, HttpStatusCode.Conflict, errorCode) { }
}

/// <summary>
/// Exception thrown when rate limit is exceeded (429)
/// </summary>
public class AllegroRateLimitException : AllegroApiException
{
    public int? RetryAfterSeconds { get; set; }
    public DateTime? RetryAfterDate { get; set; }

    public AllegroRateLimitException(string message, int? retryAfterSeconds = null) 
        : base(message, (HttpStatusCode)429)
    {
        RetryAfterSeconds = retryAfterSeconds;
        if (retryAfterSeconds.HasValue)
        {
            RetryAfterDate = DateTime.UtcNow.AddSeconds(retryAfterSeconds.Value);
        }
    }
}

/// <summary>
/// Exception thrown when server returns an error (500+)
/// </summary>
public class AllegroServerException : AllegroApiException
{
    public AllegroServerException(string message, HttpStatusCode statusCode) 
        : base(message, statusCode) { }
}

/// <summary>
/// Exception thrown when network communication fails
/// </summary>
public class AllegroNetworkException : AllegroApiException
{
    public AllegroNetworkException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when request timeout occurs
/// </summary>
public class AllegroTimeoutException : AllegroApiException
{
    public TimeSpan Timeout { get; set; }

    public AllegroTimeoutException(string message, TimeSpan timeout) : base(message)
    {
        Timeout = timeout;
    }

    public AllegroTimeoutException(string message, TimeSpan timeout, Exception innerException) 
        : base(message, innerException)
    {
        Timeout = timeout;
    }
}
