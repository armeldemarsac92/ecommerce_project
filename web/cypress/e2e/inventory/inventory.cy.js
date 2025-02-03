describe.skip('Inventory page test', () => {
    it('User can see the list of inventory', () => {
        cy.intercept('GET', '**/api/v1/products').as('getInventory');
 
        cy.visit('/dashboard')
        cy.contains('MiamMiam').should('be.visible')
        cy.visit('/dashboard/catalog/inventory')
        cy.wait('@getInventory')
        cy.get('[data-cy=inventory-table]').should('exist')
        cy.get('tr').should('have.length', 12)
        cy.get('input').should('exist')
        cy.get('button').should('exist')
    })
})