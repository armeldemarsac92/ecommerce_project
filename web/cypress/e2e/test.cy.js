describe('Test', () => {
    it('should work', () => {
        cy.visit('/')
        cy.contains('MiamMiam').should('be.visible')
        cy.visit('/dashboard/customers/users')
        cy.get('tr').should('have.length', 6)
    })
})