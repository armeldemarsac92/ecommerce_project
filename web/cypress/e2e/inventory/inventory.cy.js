describe('Inventory page test', () => {
    it('User can see the list of product', () => {
        cy.login()

        cy.intercept('GET', '**/api/v1/products', {
            fixture: "product/products.json"
        }).as('getProducts')

        cy.intercept('GET', '**/api/v1/inventories', {
            fixture: "inventory/inventories.json"
        }).as('getInventories')

        cy.contains('MiamMiam').should('be.visible')
        cy.visit('/dashboard/catalog/inventory')

        cy.wait('@getProducts')
        cy.wait('@getInventories')

        cy.get('[data-cy=inventory-table]').should('exist')
        cy.get('tr').should('have.length', 12)
        // cy.get('input').should('exist')
        // cy.contains('Next').click()
    })
})