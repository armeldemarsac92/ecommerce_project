using Tdev702.Contracts.API.Request.Brand;
using Tdev702.Contracts.API.Response;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.Mapping;
using Tdev702.Contracts.SQL.Request.All;
using Tdev702.Repository.Context;
using Tdev702.Repository.Repository;

namespace Tdev702.Api.Services;

public interface IBrandsService
{
    public Task<BrandResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<BrandResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
    public Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default);
    public Task<BrandResponse> UpdateAsync(long id, UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class BrandsService : IBrandsService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BrandsService> _logger;

    public BrandsService(
        IBrandRepository brandRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<BrandsService> logger)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<BrandResponse> GetByIdAsync(long brandId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting brand with id: {brandId}", brandId);
        var response = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
        if(response is null) throw new NotFoundException($"Brand {brandId} not found");
        
        return response.MapToBrand();
    }

    public async Task<List<BrandResponse>> GetAllAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all brands");
        var response = await _brandRepository.GetAllAsync(queryOptions, cancellationToken);
        return response.Any() ? response.MapToBrands() :  throw new NotFoundException("Brands not found");
    }

    public async Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new brand {brandName}", createBrandRequest.Title);
        var sqlRequest = createBrandRequest.MapToCreateBrandRequest();
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var createdBrandId = await _brandRepository.CreateAsync(sqlRequest, cancellationToken);
            var brandResponse = await _brandRepository.GetByIdAsync(createdBrandId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Brand {brandId} created successfully.", createdBrandId);
            return brandResponse.MapToBrand();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Error creating brand {brandName}: {message}", createBrandRequest.Title, ex.Message);
            throw;
        }
    }

    public async Task<BrandResponse> UpdateAsync(long brandId, UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating brand {brandId}", brandId);
        var sqlRequest = updateBrandRequest.MapToUpdateBrandRequest(brandId);
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var affectedRows = await _brandRepository.UpdateAsync(sqlRequest, cancellationToken);

            if (affectedRows == 0) throw new NotFoundException($"Product {brandId} not found");

            var updatedProduct = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Brand {brandId} updated successfully.", brandId);
            return updatedProduct.MapToBrand();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Error updating brand {brandId}: {message}", brandId, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(long brandId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting brand {brandId}", brandId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _brandRepository.DeleteAsync(brandId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Brand {brandId} deleted successfully.", brandId);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Error deleting brand {brandId}: {message}", brandId, ex.Message);
            throw;
        }
    }
}