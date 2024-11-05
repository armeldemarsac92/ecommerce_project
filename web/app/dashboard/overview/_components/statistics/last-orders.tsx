"use client"

import { motion, AnimatePresence } from "framer-motion";
import { ArrowRight, X } from "lucide-react";
import { useState } from "react";

export default function LastOrders() {
  const [activeTransaction, setActiveTransaction] = useState<null | TransactionType>(null);

  return (
    <div className="relative w-3/6 h-full flex items-start justify-center px-4 py-2 bg-white rounded-xl">
      <motion.div
        className="bg-white w-full h-full flex flex-col items-start overflow-hidden p-0 relative"
        style={{
          borderRadius: 20,
          //height: activeTransaction ? 300 : 408,
        }}
        layoutId="container"
      >
        <div className="w-full">
          <p className="text-sm text-neutral-400 font-semibold p-4 pb-2">
            Recent Orders
          </p>
        </div>
        <div className="w-full max-h-[26rem] flex flex-col items-center justify-center overflow-y-auto pt-24">
          {transactions.map((transaction, index) => (
            <motion.div
              key={index}
              className="flex items-center justify-between w-full gap-2 py-1.5 px-4 cursor-pointer"
              onClick={() => setActiveTransaction(transaction)}
            >
              <motion.img
                src={`https://api.dicebear.com/9.x/lorelei/svg?seed=${transaction.name}`}
                alt={transaction.name}
                className="w-12 h-12 bg-neutral-800 shrink-0 object-cover"
                style={{
                  borderRadius: 999,
                }}
                key={`avatar-${transaction.name}`}
                layoutId={`avatar-${transaction.name}`}
              />
              <motion.div
                className="w-full flex items-center justify-between gap-2"
                key={`subinfos-${transaction.name}`}
                layoutId={`subinfos-${transaction.name}`}
              >
                <div className="w-full">
                  <h3 className="text-md font-semibold">{transaction.name}</h3>
                  <p className="text-sm text-gray-400">
                    {transaction.type === "income" ? "Received" : "Sent"}
                  </p>
                </div>
                <div className="text-neutral-400 shrink-0">
                  <p className={`text-sm font-semibold`}>
                    {transaction.type === "income" ? "+" : "-"} $
                    {transaction.amount}
                  </p>
                </div>
              </motion.div>
            </motion.div>
          ))}
        </div>
        <div className="absolute bottom-0 flex items-center justify-center w-full p-4 pt-2">
          <button className="flex items-center justify-center bg-neutral-100 w-full p-2.5 rounded-xl text-sm font-medium">
            All Orders
            <ArrowRight size={18} className="ml-2" />
          </button>
        </div>
      </motion.div>


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
              <motion.img
                src={`https://api.dicebear.com/9.x/lorelei/svg?seed=${activeTransaction.name}`}
                alt={activeTransaction.name}
                className="w-12 h-12 bg-neutral-800 shrink-0 object-cover"
                style={{ borderRadius: 999 }}
                key={`avatar-${activeTransaction.name}`}
                layoutId={`avatar-${activeTransaction.name}`}
              />
              <motion.div
                className="w-full flex items-center justify-between gap-2"
                key={`subinfos-${activeTransaction.name}`}
                layoutId={`subinfos-${activeTransaction.name}`}
              >
                <div className="w-full">
                  <h3 className="text-md font-semibold">
                    {activeTransaction.name}
                  </h3>
                  <p className="text-sm text-gray-400">
                    {activeTransaction.type === "income" ? "Received" : "Sent"}
                  </p>
                </div>
                <div className="text-neutral-400 shrink-0">
                  <p className={`text-sm font-semibold`}>
                    {activeTransaction.type === "income" ? "+" : "-"} $
                    {activeTransaction.amount}
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
                <p className="text-sm font-semibold">
                  {activeTransaction.number}
                </p>
              </motion.div>
            </motion.div>
          </div>
        ) : null}
      </AnimatePresence>
    </div>
  );
}

type TransactionType = {
  name: string;
  amount: number;
  type: "income" | "expense";
  date: string;
  hour: string;
  number: string;
  card: string;
  idTransaction: number;
};

const transactions: TransactionType[] = [
  {
    name: "Leonel",
    amount: 100,
    type: "expense",
    date: "Sep 30, 2021",
    hour: "03:46 pm",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 48102,
  },
  {
    name: "Xavier",
    amount: 200,
    type: "income",
    date: "Nov 30, 2020",
    hour: "12:00 pm",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 83623,
  },
  {
    name: "LN",
    amount: 300,
    type: "income",
    date: "Oct 01, 2021",
    hour: "04:00 pm",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 92361,
  },
  {
    name: "Thomas",
    amount: 400,
    type: "expense",
    date: "May 30, 2021",
    hour: "09:13 am",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 81546,
  },
  {
    name: "Alex",
    amount: 500,
    type: "expense",
    date: "Aug 30, 2021",
    hour: "10:11 am",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 10460,
  },
  {
    name: "Test",
    amount: 500,
    type: "expense",
    date: "Aug 30, 2021",
    hour: "10:11 am",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 10460,
  },
  {
    name: "dazd",
    amount: 500,
    type: "expense",
    date: "Aug 30, 2021",
    hour: "10:11 am",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 10460,
  },
  {
    name: "CACA",
    amount: 500,
    type: "expense",
    date: "Aug 30, 2021",
    hour: "10:11 am",
    number: "**** **** **** 1234",
    card: "Visa",
    idTransaction: 10460,
  },
];

