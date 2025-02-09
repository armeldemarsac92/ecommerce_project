describe('Order page test', () => {
    it('User can see orders', () => {
        cy.login()

        cy.intercept('GET', '**/api/v1/orders', {
            fixture: 'order/orders.json',
        }).as('getOrders')
        cy.intercept('GET', '**/api/v1/customers', {
            fixture: 'customer/customers.json',
        }).as('getCustomers')
        cy.intercept('GET', '**/api/v1/orders/5/invoice', {
            statusCode: 200,
            body: null
        }).as('getInvoices')

        cy.contains('MiamMiam').should('be.visible')
        cy.visit('/dashboard/sales/orders')

        cy.wait('@getOrders')
        cy.wait('@getCustomers')

        cy.get('[data-cy="order-table"]').should('exist')
        cy.get('[data-cy="order-item"]').eq(7).click()
        cy.get('[data-cy="invoice-button"]').should('be.visible')
    })
})
