"use client"

import { motion, AnimatePresence } from "framer-motion";
import { ArrowRight, X } from "lucide-react";
import { useEffect, useState } from "react";
import { ScrollShadow } from "@nextui-org/react";
import { Button } from "@/components/shadcn/button";

export default function LastOrders() {
  const [activeTransaction, setActiveTransaction] = useState<null | TransactionType>(null);
  const [newTransactionsList, setNewTransactionsList] = useState<TransactionType[]>([]);
  const [test, setTest] = useState(0);

  useEffect(() => {
    let timeoutId: NodeJS.Timeout;

    if(test < transactions.length) {
      timeoutId = setTimeout(() => {
        setNewTransactionsList(prev => [...prev, transactions[test]]);
        setTest(test + 1);
      }, 1000);
    }

    return () => {
      if (timeoutId) {
        clearTimeout(timeoutId);
      }
    };
  }, [test]);

  return (
    <div className="relative w-3/6 h-full flex items-start justify-center px-4 py-2 bg-white rounded-xl">
      <motion.div
        className="bg-white w-full h-full flex flex-col items-start overflow-hidden p-0 relative"
        style={{
          borderRadius: 20,
          /*height: activeTransaction ? 300 : 408,*/
        }}
        layoutId="container"
      >
        <div className="w-full">
          <p className="text-muted-foreground font-semibold p-4 pb-2">
            Last Orders
          </p>
        </div>
        <ScrollShadow className="w-full h-[440px]" orientation={"vertical"}>
          <div className="flex flex-col-reverse w-full gap-y-2 items-center justify-center py-2">
            {newTransactionsList.map((transaction, index) => (
              <motion.div
                key={index}
                className="flex items-center justify-between w-full gap-2 py-1.5 px-4 cursor-pointer"
                onClick={() => setActiveTransaction(transaction)}
                initial={{ opacity: 0, translateY: -20 }}
                animate={{ opacity: 1, translateY: 0 }}
              >
                {/*<motion.img
                  src={`https://api.dicebear.com/9.x/lorelei/svg?seed=${transaction.name}`}
                  alt={transaction.name}
                  className="w-12 h-12 bg-neutral-800 shrink-0 object-cover"
                  style={{
                    borderRadius: 999,
                  }}
                  key={`avatar-${transaction.name}`}
                  layoutId={`avatar-${transaction.name}`}
                />*/}
                <motion.div
                  className="w-full flex items-center justify-between gap-2"
                  key={`subinfos-${transaction.name}`}
                  layoutId={`subinfos-${transaction.name}`}
                >
                  <div className="w-full space-y-1">
                    <h3 className="text-sm font-bold">{transaction.name}</h3>
                    <p className="text-xs text-gray-400">
                      Received
                    </p>
                  </div>
                  <div className="flex flex-col items-end text-success shrink-0 gap-y-1">
                    <p className={`text-sm font-semibold`}>
                      + {transaction.amount}€
                    </p>
                    <p className={"text-[11px] text-black/20 font-light"}>{`${transaction.hour} • ${transaction.date}`}</p>
                  </div>
                </motion.div>
              </motion.div>
            ))}
          </div>
        </ScrollShadow>

        <div className="absolute bottom-0 flex items-center justify-center w-full p-4 pt-2">
          <Button className={"w-full"} variant="expandIcon" Icon={<ArrowRight size={15}/>} iconPlacement="right">
            All Orders
          </Button>
        </div>
      </motion.div>

      {/* DETAIL CARD */}
      <AnimatePresence>
        {activeTransaction ? (
          <div className="inset-0 absolute w-full h-full flex items-center justify-center">
            <motion.div
              className="w-[350px] p-4 bg-white relative flex flex-col items-start gap-2"
              style={{
                borderRadius: 20,
              }}
              layoutId="container"
            >
              <motion.div className="absolute right-2 top-2 z-10" layout>
                <button
                  onClick={() => setActiveTransaction(null)}
                  className="bg-neutral-100 p-1"
                  style={{
                    borderRadius: 999,
                  }}
                >
                  <X size={20} />
                </button>
              </motion.div>
              {/*<motion.img
                src={`https://api.dicebear.com/9.x/lorelei/svg?seed=${activeTransaction.name}`}
                alt={activeTransaction.name}
                className="w-12 h-12 bg-neutral-800 shrink-0 object-cover"
                style={{ borderRadius: 999 }}
                key={`avatar-${activeTransaction.name}`}
                layoutId={`avatar-${activeTransaction.name}`}
              />*/}
              <motion.div
                className="w-full flex items-center justify-between gap-2 mt-6"
                key={`subinfos-${activeTransaction.name}`}
                layoutId={`subinfos-${activeTransaction.name}`}
              >
                <div className="w-full">
                  <h3 className="text-md font-semibold">
                    {activeTransaction.name}
                  </h3>
                  <p className="text-sm text-gray-400">
                    Received
                  </p>
                </div>
                <div className="text-success shrink-0">
                  <p className={`text-sm font-semibold`}>
                    + {activeTransaction.amount}€
                  </p>
                </div>
              </motion.div>
              <motion.div
                layout
                className="h-[1px] w-full border-b border-dashed border-neutral-200"
              ></motion.div>
              <motion.div
                layout
                className="flex flex-col items-start w-full gap-2"
              >
                <p className="text-sm text-gray-400">
                  #{activeTransaction.idTransaction}
                </p>
                <p className="text-sm font-semibold">
                  {activeTransaction.date}
                </p>
                <p className="text-sm font-semibold">
                  {activeTransaction.hour}
                </p>
              </motion.div>
              <motion.div
                layout
                className="h-[1px] w-full border-b border-dashed border-neutral-200"
              ></motion.div>
              <motion.div
                layout
                className="flex flex-col items-start w-full gap-2"
              >
                <p className="text-sm text-gray-400">Paid via Credit Card</p>
              </motion.div>
            </motion.div>
          </div>
        ) : null}
      </AnimatePresence>
      {/* END DETAIL CARD*/}

    </div>
  );
}

type TransactionType = {
  name: string;
  amount: number;
  date: string;
  hour: string;
  card: string;
  idTransaction: number;
};

const transactions: TransactionType[] = [
  {
    name: "Leonel",
    amount: 35.99,
    date: "Nov 02, 2024",
    hour: "03:46 pm",
    card: "Visa",
    idTransaction: 48102,
  },
  {
    name: "Xavier",
    amount: 23.99,
    date: "Oct 30, 2024",
    hour: "12:00 pm",
    card: "Visa",
    idTransaction: 83623,
  },
  {
    name: "Carlos",
    amount: 20.00,
    date: "Oct 29, 2024",
    hour: "04:00 pm",
    card: "Visa",
    idTransaction: 92361,
  },
  {
    name: "Thomas",
    amount: 49.99,
    date: "Oct 10, 2024",
    hour: "09:13 am",
    card: "Visa",
    idTransaction: 81546,
  },
  {
    name: "Alex",
    amount: 12.99,
    date: "Oct 02, 2024",
    hour: "10:11 am",
    card: "Visa",
    idTransaction: 10460,
  },
  {
    name: "John",
    amount: 14.00,
    date: "Sep 30, 2024",
    hour: "10:11 am",
    card: "Visa",
    idTransaction: 10460,
  },
  {
    name: "David",
    amount: 12.30,
    date: "Sep 25, 2024",
    hour: "10:11 am",
    card: "Visa",
    idTransaction: 10460,
  },
  {
    name: "Daniel",
    amount: 34.55,
    date: "Sep 04, 2024",
    hour: "10:11 am",
    card: "Visa",
    idTransaction: 10460,
  },
];

