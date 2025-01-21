using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Repository.Brands;

namespace Tdev702.Api.Services;

public interface IBrandsService
{
    public Task<BrandResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<BrandResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest,
        CancellationToken cancellationToken = default);

    public Task<BrandResponse> UpdateAsync(long id, UpdateBrandRequest updateBrandRequest,
        CancellationToken cancellationToken = default);

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

public class BrandsService : IBrandsService
{
    private readonly IBrandRepository _brandRepository;
    private readonly ILogger<BrandsService> _logger;

    public BrandsService(IBrandRepository brandRepository, ILogger<BrandsService> logger)
    {
        _brandRepository = brandRepository;
        _logger = logger;
    }

    public async Task<BrandResponse> GetByIdAsync(long brandId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to fetch Brand: {brandId}", brandId);
            var response = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
            if (response is null) throw new NotFoundException($"Brand {brandId} not found");
            _logger.LogInformation("Successfully fetched Brand: {brandId}", brandId);
            return response.MapToBrand();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting Brand by Id: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<List<BrandResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to fetch all Brands");
            var response = await _brandRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Successfully fetched all Brands");
            return response.Any() ? response.MapToBrands() : throw new NotFoundException("Brands not found");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting Brands: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to create Brand: {Brand}", createBrandRequest.Title);
            var sqlRequest = createBrandRequest.MapToCreateBrandRequest();
            var response = await _brandRepository.CreateAsync(sqlRequest, cancellationToken);
            _logger.LogInformation("Successfully created Brand: {Brand}", createBrandRequest.Title);
            return response.MapToBrand();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating Brand: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<BrandResponse> UpdateAsync(long brandId, UpdateBrandRequest updateBrandRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to modify Brand: {BrandId}", brandId);
            var sqlRequest = updateBrandRequest.MapToUpdateBrandRequest(brandId);
            var affectedRows = await _brandRepository.UpdateAsync(sqlRequest, cancellationToken);
            if (affectedRows == 0) throw new NotFoundException($"Product {brandId} not found");
            var updatedProduct = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
            _logger.LogInformation("Successfully updated Brand: {BrandId}", brandId);
            return updatedProduct.MapToBrand();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating Brand: {Message}", ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(long brandId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Trying to delete Brand: {BrandId}", brandId);
            await _brandRepository.DeleteAsync(brandId, cancellationToken);
            _logger.LogInformation("Successfully deleted Brand: {BrandId}", brandId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting Brand: {Message}", ex.Message);
            throw;
        }
    }
}