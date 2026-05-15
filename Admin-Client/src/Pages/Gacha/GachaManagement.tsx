import { AppSidebar } from "@/components/app-sidebar";
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import {columns, type GachaItems } from "./PricePool/columns";
import { DataTable } from "./PricePool/data-table";
import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";

import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
const API = import.meta.env.VITE_API_BASE_URL

export default function GachaManagement(){
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
                <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">BB-Food | Add Gacha Item</h1>
                <div className="container mx-auto py-10">

                <div className="flex gap-4">
                  <Dialog>
                    <DialogTrigger asChild>
                      <Button variant="outline">Add Gacha</Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogTitle>Add Gacha</DialogTitle>
                      <DialogDescription>
                        Insert the ID, Item name, description and rarity to add a new Gacha item. 
                      </DialogDescription>
                      <div className="flex gap-3">
                        <div className="w-1/5">
                          <Label>ID</Label>
                          <Input className="mt-2"/>
                        </div>
                        <div className="w-1/5">
                          <Label>Rarity</Label>
                          <Input className="mt-2"/>
                        </div>
                        <div className="w-full">
                          <Label>Item Name</Label>
                          <Input className="mt-2"/>
                        </div>
                      </div>
                      <div>
                        <Label>Description</Label>
                        <Textarea className="mt-2"/>
                      </div>
                    </DialogContent>
                  </Dialog>

                  <Dialog>
                    <DialogTrigger asChild>
                      <Button variant="outline">Delete Gacha</Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogTitle></DialogTitle>
                    </DialogContent>
                  </Dialog>

                  <Dialog>
                    <DialogTrigger asChild>
                      <Button variant="outline">Delete All Gacha</Button>
                    </DialogTrigger>
                    <DialogContent>
                      <DialogTitle></DialogTitle>
                    </DialogContent>
                  </Dialog>
                </div>

                <DataTable columns={columns} data={items} />
                </div>
              <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
            </main>
        </SidebarProvider>
    )
}

