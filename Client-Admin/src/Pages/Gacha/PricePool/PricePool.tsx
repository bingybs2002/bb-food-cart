import { AppSidebar } from "@/components/app-sidebar";
import { Sidebar, SidebarFooter, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import {columns, type GachaItems } from "./columns";
import { DataTable } from "./data-table";
import { useEffect, useState } from "react";
import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/components/ui/drawer"
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

const API = import.meta.env.VITE_API_BASE_URL


export default function GachaPricePool(){

  const [items, setItems] = useState<GachaItems[]>([])

  useEffect(()=>{
    fetch(`${API}/Gacha/all-items`)
    .then((res)=>res.json())
    .then((data: GachaItems[])=>setItems(data))
  }, [])

  return(
      <SidebarProvider>
          <AppSidebar/>
          <main className="relative w-full space-y-6 p-6">

            <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB-Food | Price Pool</h1>
            <div className="container mx-auto py-10">
            </div>
            <h2>Gacha Items are listed below:</h2>
            <DataTable columns={columns} data={items} />

            <div className="flex justify-end border-white">
                <Drawer>
                  <Button>
                      <DrawerTrigger>Open Drawing Menu</DrawerTrigger>
                  </Button>
                  <DrawerContent>
                    <DrawerHeader>
                      <DrawerTitle>
                        Gacha Draw!
                      </DrawerTitle>
                      <DrawerDescription>Admin only.</DrawerDescription>
                    </DrawerHeader>
                      <div className="mx-auto flex w-3/5 justify-center items-center gap-4">
                          <Input type="password" placeholder="Enter administrator password: "/>
                          <Button>Enter</Button>
                      </div>
                  </DrawerContent>
                </Drawer>
            </div>

          <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />

          </main>
      </SidebarProvider>
  )
}

