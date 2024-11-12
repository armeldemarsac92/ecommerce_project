"use client";

import { Card, CardBody, CardFooter, Image } from "@nextui-org/react"
import { Chip } from "@nextui-org/chip";
import {useRouter} from "next/navigation";

export default function ProductsPage() {

  const router = useRouter()
  const products = [
    {
        id: 1,
        title: 'Coca Cola',
        price: '1.50€',
        stock: 10,
        image: '/images/coca-cola.jpg'
    },
    {
        id: 2,
        title: 'Fanta',
        price: '1.50€',
        stock: 0,
        image: '/images/fanta.jpeg'
    },
    {
        id: 3,
        title: 'Banana',
        price: '1.50€',
        stock: 1,
        image: '/images/banana.jpeg'
    },
    {
        id: 4,
        title: 'Saucisson',
        price: '1.50€',
        stock: 0,
        image: '/images/saucisson.jpeg'
    },
    {
        id: 5,
        title: 'Pain',
        price: '200€',
        stock: 10,
        image: '/images/pain.jpeg'
    }
  ]

  return (
      <div className="gap-5 grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 p-4">
          {products.map((product, index) => (
                  <Card
                      shadow="sm"
                      key={index}
                      isPressable
                      onPress={() => {
                            router.push(`/dashboard/catalog/inventory/products/${product.id}`)
                      }}
                  >
                      <CardBody className="overflow-visible p-0">
                          <Image
                              shadow="sm"
                              radius="lg"
                              width="100%"
                              fallbackSrc="/images/product-placeholder.jpeg"
                              alt={product.title}
                              className="h-[250px] object-contain"
                              src={product.image}
                          />
                      </CardBody>
                      <CardFooter className="text-small justify-between">
                          <b className="flex-1 text-left">{product.title}</b>
                          <Chip
                              size="sm"
                              color={`${product.stock > 0 ? 'success' : 'danger'}`}
                              variant="flat"
                          >
                              {product.stock > 0 ? 'In stock' : 'Out stock'}
                          </Chip>
                      </CardFooter>
                      <p className="text-default-500 p-3">
                          {product.price}
                      </p>
                  </Card>
          ))}
      </div>
  )
}
