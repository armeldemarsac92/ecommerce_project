describe.skip('Sidenav component test', () => {
    it('User can see all sidenav elements', () => {
        cy.visit('/dashboard')
        cy.get('[data-cy="navbar-header"]').should('exist')

        cy.get('[data-cy="navbar-content"]').should('exist')
        cy.contains('Overview').should('be.visible')
        cy.contains('Statistics').should('be.visible')
        cy.contains('Products').should('be.visible')
        cy.contains('Inventory').should('be.visible')
        cy.contains('Users').should('be.visible')
        cy.contains('Orders').should('be.visible')
        cy.contains('Invoices').should('be.visible')

        cy.get('[data-cy="navbar-footer"]').should('exist')
    })
})