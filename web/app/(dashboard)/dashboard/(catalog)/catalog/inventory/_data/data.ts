const columns = [
    {name: "NAME", uid: "name", sortable: true},
    {name: "SKU", uid: "sku", sortable: true},
    {name: "PRICE", uid: "price", sortable: true},
    {name: "STOCK", uid: "stock", sortable: true},
    {name: "CATEGORY", uid: "category", sortable: true},
    {name: "ACTIONS", uid: "actions"},
];

const statusOptions = [
    {name: "In stock", uid: true},
    {name: "Out stock", uid: false},
];

const products = [
    {
        id: 1,
        logo: "",
        name: "Coca cola",
        sku: "15423145",
        price: "2€",
        stock: 0,
        category: "Drink"
    },
    {
        id: 2,
        logo: "",
        name: "Bacon",
        sku: "49846233145",
        price: "4€",
        stock: 10,
        category: "Meat"
    },
    {
        id: 3,
        logo: "",
        name: "Water 1L",
        sku: "154231454215",
        price: "1€",
        stock: 10,
        category: "Drink"
    },
    {
        id: 4,
        logo: "",
        name: "Pizza express margherita",
        sku: "1545223145",
        price: "5€",
        stock: 0,
        category: "Pizzas pies and quiches"
    },
    {
        id: 5,
        logo: "",
        name: "Dark chocolate 65% cocoa",
        sku: "15423145",
        price: "2€",
        stock: 1,
        category: "Snacks"
    }
];

export {columns, products, statusOptions};