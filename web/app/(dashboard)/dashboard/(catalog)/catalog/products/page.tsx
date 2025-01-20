"use client";

import {Card, CardBody, CardFooter, Image, Pagination} from "@nextui-org/react"
import { Chip } from "@nextui-org/chip";
import {SearchBar} from "@/components/ui/search-bar";
import React, {useEffect, useState} from "react";
import {Button} from "@/components/shadcn/button";
import {ChevronLeft, ChevronRight} from "lucide-react";
import {Spinner} from "@nextui-org/react";
import {Link} from "@nextui-org/link";
import {getProducts} from "@/actions/products/products";
import {useToast} from "@/hooks/use-toast";
import Product from "@/types/product";

export default function ProductsPage() {
    const [filterValue, setFilterValue] = useState("");
    const [loading, isLoading] = useState(true);
    const [page, setPage] = useState(1);
    const [rowsPerPage] = useState(10);
    const [products, setProducts] = useState<Product[]>([]);
    const { toast } = useToast()

    useEffect(() => {
        getProducts().then((response) => {
            setProducts(response.data);
            console.log(response);
            isLoading(false);
        }).catch(() => {
            toast({ variant: "destructive", title: "Error", description: "An error occurred"})
        })


    }, []);

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
              {loading ? (
                  <div className="h-[30rem] w-full flex justify-center">
                    <Spinner label="Loading..." color="success" labelColor="success" size="lg" />
                  </div>
              ) : (
                  <div className="gap-5 grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 p-4">
                      {filteredItems.map((product, index) => (
                          <Card
                              shadow="sm"
                              key={index}
                              as={Link}
                              href={`/dashboard/catalog/inventory/products/${product.id}`}
                          >
                              <CardBody className="overflow-visible p-0">
                                  <Image
                                      shadow="sm"
                                      radius="lg"
                                      width="100%"
                                      alt={product.title}
                                      className="h-[250px] object-contain"
                                      src={product.image || '/images/product-placeholder.jpeg'}
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
                                  {product.price}€
                              </p>
                          </Card>
                      ))}
                  </div>
              )}
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
