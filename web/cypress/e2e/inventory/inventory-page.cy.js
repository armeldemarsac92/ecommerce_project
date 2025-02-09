describe('Inventory page test', () => {
    it('User can see the list of product and delete one', () => {
        cy.login()
        cy.intercept('GET', '**/api/v1/products', {
            fixture: "product/products.json"
        }).as('getProducts')

        cy.intercept('GET', '**/api/v1/inventories', {
            fixture: "inventory/inventories.json"
        }).as('getInventories')

        cy.intercept('DELETE', '**/api/v1/products/**', {
            statusCode: 204,
            body: null
        }).as('deleteProduct')

        cy.visit('/dashboard/catalog/inventory')

        cy.wait('@getProducts')
        cy.wait('@getInventories')
        cy.get('input').should('exist')
        cy.contains('Next').click()
        cy.get('[data-cy=actions-dropdown]').eq(0).click()
        cy.contains('Delete').click()
        cy.get('[data-cy="delete-modal"]').should('be.visible')
        cy.get('[data-cy="delete-button"]').click({force: true})
        cy.wait('@deleteProduct')
        cy.get('[data-cy="inventory-table"]').should('be.visible')
    })

    it('User can see stock modal and manage it', () => {
        cy.login()
        cy.intercept('GET', '**/api/v1/products', {
            fixture: "product/products.json"
        }).as('getProducts')

        cy.intercept('GET', '**/api/v1/inventories', {
            fixture: "inventory/inventories.json"
        }).as('getInventories')

        cy.intercept('DELETE', '**/api/v1/products/**', {
            statusCode: 200,
            body: null
        }).as('deleteProduct')


        cy.intercept('PUT', '**/api/v1/inventories/13/increament', {
            statusCode: 200,
            body: null
        }).as('updateStock')

        cy.visit('/dashboard/catalog/inventory')

        cy.wait('@getProducts')
        cy.wait('@getInventories')

        cy.get('[data-cy=actions-dropdown]').eq(0).click()
        cy.contains('Manage quantity').click()
        cy.get('[data-cy="stock-modal"]').should('be.visible')
        cy.get('[data-cy="quantity-input"]').type(1)
        cy.get('[data-cy="update-quantity-input"]').click()
        cy.wait('@updateStock')
        cy.wait('@getInventories')
        cy.get('[data-cy="inventory-table"]').should('be.visible')
    })
})
