using Tdev702.Contracts.API.Shop;
using Tdev702.Contracts.Exceptions;
using Tdev702.Contracts.SQL.Mapping;
using Tdev702.Contracts.SQL.Request.Shop.Brand;
using Tdev702.Repository.Brands;

namespace Tdev702.Api.Services;

public interface IBrandsService
{
    public Task<Brand> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<List<Brand>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Brand> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default);
    public Task<Brand> UpdateAsync(long id, UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default);
    public Task DeleteAsync(long id ,CancellationToken cancellationToken = default);
}

public class BrandsService : IBrandsService
{
    private readonly IBrandRepository _brandRepository;

    public BrandsService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }
    
    public async Task<Brand> GetByIdAsync(long brandId, CancellationToken cancellationToken = default)
    {
        var response = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
        if(response is null) throw new NotFoundException($"Brand {brandId} not found");
        
        return response.MapToBrand();
    }

    public async Task<List<Brand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _brandRepository.GetAllAsync(cancellationToken);
        return response.Any() ? response.MapToBrands() :  throw new NotFoundException("Brands not found");
    }

    public async Task<Brand> CreateAsync(CreateBrandRequest createBrandRequest, CancellationToken cancellationToken = default)
    {
        var response = await _brandRepository.CreateAsync(createBrandRequest, cancellationToken);
        return response.MapToBrand();
    }

    public async Task<Brand> UpdateAsync(long brandId, UpdateBrandRequest updateBrandRequest, CancellationToken cancellationToken = default)
    {
        updateBrandRequest.BrandId = brandId;
        var affectedRows = await _brandRepository.UpdateAsync(updateBrandRequest, cancellationToken);

        if (affectedRows == 0) throw new NotFoundException($"Product {brandId} not found");
        
        var updatedProduct = await _brandRepository.GetByIdAsync(brandId, cancellationToken);
        return updatedProduct.MapToBrand();
        
    }

    public async Task DeleteAsync(long brandId, CancellationToken cancellationToken = default)
    {
        await _brandRepository.DeleteAsync(brandId, cancellationToken);
    }
}