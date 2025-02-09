describe('Catalog page test', () => {
    it('User can see catalog page and create a product', () => {
        cy.login()

        cy.intercept('GET', '**/api/v1/products', {
            fixture: "product/products.json"
        }).as('getProducts')

        cy.intercept('GET', '**/api/v1/inventories', {
            fixture: "inventory/inventories.json"
        }).as('getInventories')

        cy.intercept('GET', '**/api/v1/open_food_fact?Category=carotte&NutritionGrade=A&PageSize=15', {
            fixture: "open_food_fact/products.json"
        }).as('getOpenFoodFactProducts')

        cy.intercept('GET', '**/api/v1/products_tags', {
            fixture: "product-tag/product-tags.json"
        }).as('getProductTags')

        cy.intercept('GET', '**/api/v1/categories', {
            fixture: "category/categories.json"
        }).as('getCategories')

        cy.intercept('GET', '**/api/v1/brands', {
            fixture: "brand/brands.json"
        }).as('getBrands')

        cy.intercept('POST', '**/api/v1/products', {
            statusCode: 200,
            body: null
        }).as('createProduct')

        cy.visit('/dashboard/catalog/products')
        cy.wait('@getProducts')
        cy.wait('@getInventories')

        cy.get('[data-cy="catalog-header"]').should('be.visible')
        cy.get('button').contains('Add product').click()
        cy.get('[data-cy="search-input"]').type('carotte')
        cy.get('[data-cy="nutrition-grade-dropdown"]').click()
        cy.get('[role="option"]').contains('A').click()
        cy.get('[data-cy="search-button"]').click()
        cy.get('@getOpenFoodFactProducts')
        cy.get('[data-cy="product-card"]').eq(0).click()
        cy.wait('@getProductTags')
        cy.wait('@getCategories')
        cy.wait('@getBrands')
        cy.get('[data-cy="title-input"]').type('carotte')
        cy.get('[data-cy="description-input"]').type('carotte rapées')
        cy.get('[data-cy="price-input"]').type(2)
        cy.get('[data-cy="category-trigger"]').click()
        cy.get('[role="option"]').contains('test cat14').click()
        cy.get('[data-cy="brand-trigger"]').click()
        cy.get('[role="option"]').contains('Lipton').click()
        cy.get('[data-cy="quantity-input"]').type(2)
        cy.get('[data-cy="submit-button"]').click()
        cy.wait('@createProduct')
    })

    it('User can modify products', () => {
        cy.login()

        cy.intercept('GET', '**/api/v1/products', {
            fixture: "product/products.json"
        }).as('getProducts')

        cy.intercept('GET', '**/api/v1/inventories', {
            fixture: "inventory/inventories.json"
        }).as('getInventories')

        cy.intercept('GET', '**/api/v1/products_tags', {
            fixture: "product-tag/product-tags.json"
        }).as('getProductTags')

        cy.intercept('GET', '**/api/v1/categories', {
            fixture: "category/categories.json"
        }).as('getCategories')

        cy.intercept('GET', '**/api/v1/brands', {
            fixture: "brand/brands.json"
        }).as('getBrands')

        cy.intercept('GET', '**/api/v1/products/*', {
            fixture: "product/product-to-modify.json"
        }).as('getProductToModify')

        cy.intercept('PUT', '**/api/v1/products/*?productId=*', {
            statusCode: 200,
            body: null
        }).as('modifyProduct')

        cy.visit('/dashboard/catalog/products')
        cy.wait('@getProducts')
        cy.wait('@getInventories')

        cy.get('[data-cy="catalog-item"]').eq(0).click()
        cy.wait('@getProductTags')
        cy.wait('@getCategories')
        cy.wait('@getBrands')
        cy.wait('@getProductToModify')
        cy.get('[data-cy="title-input"]').type('carotte')
        cy.get('[data-cy="submit-button"]').click()
        cy.wait('@modifyProduct')
    })
})