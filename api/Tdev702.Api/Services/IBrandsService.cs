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
    public Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default);
    public Task<BrandResponse> UpdateAsync(long id, UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class BrandsService : IBrandsService
{
    private readonly IBrandRepository _brandRepository;

    public BrandsService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }
    
    public async Task<BrandResponse> GetByIdAsync(long brandId, CancellationToken cancellationToken = default)
    {
        var response = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
        if(response is null) throw new NotFoundException($"Brand {brandId} not found");
        
        return response.MapToBrand();
    }

    public async Task<List<BrandResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _brandRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToBrands() :  throw new NotFoundException("Brands not found");
    }

    public async Task<BrandResponse> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default)
    {
        var sqlRequest = createBrandRequest.MapToCreateBrandRequest();
        var response = await _brandRepository.CreateAsync(sqlRequest, cancellationToken);
        return response.MapToBrand();
    }

    public async Task<BrandResponse> UpdateAsync(long brandId, UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default)
    {
        var sqlRequest = updateBrandRequest.MapToUpdateBrandRequest(brandId);
        var affectedRows = await _brandRepository.UpdateAsync(sqlRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Product {brandId} not found");
        
        var updatedProduct = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
        return updatedProduct.MapToBrand();
        
    }

    public async Task DeleteAsync(long brandId, CancellationToken cancellationToken = default)
    {
        await _brandRepository.DeleteAsync(brandId, cancellationToken);
    }
}