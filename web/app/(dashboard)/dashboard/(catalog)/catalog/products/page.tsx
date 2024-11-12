"use client";

import {Card, CardBody, CardFooter, Image, Pagination} from "@nextui-org/react"
import { Chip } from "@nextui-org/chip";
import {useRouter} from "next/navigation";
import {SearchBar} from "@/components/ui/search-bar";
import React from "react";
import {Button} from "@/components/shadcn/button";
import {ChevronLeft, ChevronRight} from "lucide-react";

export default function ProductsPage() {
  const [filterValue, setFilterValue] = React.useState("");
  const [page, setPage] = React.useState(1);
    const [rowsPerPage] = React.useState(10);
    const router = useRouter();
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
  ];

    const hasSearchFilter = Boolean(filterValue);

    const filteredItems = React.useMemo(() => {
        let filteredUsers = [...products];

        if (hasSearchFilter) {
            filteredUsers = filteredUsers.filter((product) =>
                product.title.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }

        return filteredUsers;
      }, [products, filterValue]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

    const onNextPage = React.useCallback(() => {
        if (page < pages) {
            setPage(page + 1);
        }
    }, [page, pages]);

    const onPreviousPage = React.useCallback(() => {
        if (page > 1) {
            setPage(page - 1);
        }
    }, [page]);

    const onSearchChange = React.useCallback((value: string) => {
        if (value) {
          setFilterValue(value);
          setPage(1);
        } else {
          setFilterValue("");
        }
    }, []);

    const onClear = React.useCallback(() => {
        setFilterValue("");
        setPage(1);
    }, []);

  return (
      <div>
          <div className="flex flex-col">
              <div className="m-4">
                  <SearchBar onClear={onClear} onValueChange={onSearchChange} value={filterValue}/>
              </div>
              <div className="gap-5 grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 p-4">
                  {filteredItems.map((product, index) => (
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
          </div>
          <div className="py-2 px-2 flex justify-between items-center m-2">
              <Pagination
                classNames={{
                    cursor: "bg-secondary rounded-sm",
                    prev: "rounded-sm",
                    next: "rounded-sm",
                }}
                radius={"sm"}
                isCompact
                showControls
                showShadow
                page={page}
                total={pages}
                onChange={setPage}
            />
            <div className="hidden sm:flex w-[30%] justify-end gap-2">
                    <Button variant={"expandIcon"} iconPlacement={"left"} Icon={ChevronLeft} disabled={pages === 1} size="sm"
                            onClick={onPreviousPage}>
                        Previous
                    </Button>
                    <Button variant={"expandIcon"} iconPlacement={"right"} Icon={ChevronRight} disabled={pages === 1} size="sm"
                            onClick={onNextPage}>
                        Next
                    </Button>
              </div>
            </div>
      </div>
    )
}
