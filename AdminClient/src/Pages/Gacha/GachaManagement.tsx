import { AppSidebar } from "@/components/app-sidebar";
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import {columns, type GachaItems } from "./PricePool/columns";
import { DataTable } from "./PricePool/data-table";
import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import {Dialog, DialogClose, DialogContent, DialogDescription, DialogTitle, DialogTrigger} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
const API = import.meta.env.VITE_API_BASE_URL

export default function GachaManagement(){
  const [Pagination, setPagination] = useState({
    pageIndex: 0,
    pageSize: 10,
  })
  const [items, setItems] = useState<GachaItems[]>([])

  useEffect(()=>{
    fetch(`${API}/Gacha/all-items`)
    .then((res)=>res.json())
    .then((data: GachaItems[])=>setItems(data))
  }, [])

  const fetchGachaItems = async () => {
    const res = await fetch(`${API}/Gacha/all-items`);
    if (!res.ok) {
      throw new Error("Failed to fetch items");
    }
    const data: GachaItems[] = await res.json();
    setItems(data);
};

  const [addGachaForm, setAddGachaForm] = useState({
    id: 0,
    rarity: 0,
    itemName: '',
    description: ''
  })

  const [DeleteGachaById, setDeleteGachaById] = useState({
    id: 0
  })

const AddGachaRequest = async () => {
  try {
    const res = await fetch(`${API}/Gacha/insert`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      },
      body: JSON.stringify([addGachaForm]),
    });
    if (!res.ok) {
      throw new Error("Failed to add item");
    }
    const message = await res.text();
    console.log(message);
    fetchGachaItems(); 
  } catch (error) {
    console.error("Error adding gacha item:", error);
  }
};

  const DeleteGachaByIdRequest = async() => {
    try{
      const res = await fetch(`${API}/Gacha/delete/${DeleteGachaById.id}`,{
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
        },
      })
      const message = await res.text(); 
      console.log(message);
      fetchGachaItems();
    } catch (error) {
      console.error("Error deleting gacha item:", error);
    }
  }

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
                    <Input 
                      className="mt-2"
                      value={addGachaForm.id}
                      onChange={(e) => setAddGachaForm({...addGachaForm, id: Number(e.target.value)})}
                      />
                  </div>
                  <div className="w-1/5">
                    <Label>Rarity</Label>
                    <Input 
                      className="mt-2"
                      value={addGachaForm.rarity}
                      onChange={(e) => setAddGachaForm({...addGachaForm, rarity:Number(e.target.value)})}
                    />
                  </div>
                  <div className="w-full">
                    <Label>Item Name</Label>
                    <Input 
                      className="mt-2"
                      value={addGachaForm.itemName}
                      onChange={(e) => setAddGachaForm({...addGachaForm, itemName: e.target.value})}
                    />
                  </div>
                </div>
                <div>
                  <Label>Description</Label>
                  <Textarea 
                    className="mt-2" value={addGachaForm.description} onChange={(e)=>setAddGachaForm({...addGachaForm, description: e.target.value})}
                  />
                </div>
                <DialogClose>
                  <Button
                    onClick={AddGachaRequest}
                  >
                    Submit
                  </Button>
                </DialogClose>
              </DialogContent>
            </Dialog>

            <Dialog>
              <DialogTrigger asChild>
                <Button variant="outline">Delete Gacha</Button>
              </DialogTrigger>
              <DialogContent>
                <DialogTitle>Delete Gacha by ID</DialogTitle>
                <Input
                  placeholder="Enter Gacha ID to delete"
                  value={DeleteGachaById.id}
                  onChange={(e) => setDeleteGachaById({...DeleteGachaById, id: Number(e.target.value)})}
                />
                <DialogClose asChild>
                  <Button
                    onClick={DeleteGachaByIdRequest}
                  >
                    Delete
                  </Button>
                </DialogClose> 
              </DialogContent>
            </Dialog>

          </div>
            <div className="mt-5">
              <DataTable columns={columns} data={items}/>
            </div>
          </div>
        <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}