import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"
import { Button } from "@/components/ui/button"
import { useState } from "react";
import { Separator } from "@/components/ui/separator";
import { Calendar } from "@/components/ui/calendar";
import React from "react";
import { Bar, BarChart, CartesianGrid , XAxis} from "recharts";
import { ChartContainer, ChartLegend, ChartLegendContent, ChartTooltip, ChartTooltipContent, type ChartConfig } from "@/components/ui/chart";


export default function HomePage() {
  const [showImage, setShowImage] = useState(false)
  const [date, setDate] = React.useState<Date | undefined>(new Date())

  const chartConfig = {
    Visits: {
      label: "Visits",
      color: "#2563eb",
    },
    Orders: {
      label: "Orders",
      color: "#60a5fa",
    },
  } satisfies ChartConfig

  const today = new Date()
  const chartData = Array.from({ length: 7 }, (_, i) => {
    const date = new Date()
    date.setDate(date.getDate() - (6 - i))

    return {
      month: date.toLocaleDateString("en-US", {
        weekday: "short", 
      }),
      Visits: Math.floor(Math.random() * 300),
      Orders: Math.floor(Math.random() * 200),
    }
  })
  return (
    <SidebarProvider>
      <AppSidebar />

      <main className="w-full relative">
        <h1 className="mt-8 text-center text-4xl font-bold">Admin Dashboard  |  BB Food Cart</h1>
        <div className="flex justify-end">
          <div className="w-60 border-red-500 border-2 rounded-lg p-4 m-4">
            <h3 className="text-center font-semibold">Our Cart Address:</h3>
            <p className="text-right text-stone-100">
              7350 Bustleton Avenue, <br/>
              Philadelphia, 19149
            </p>
          </div>

        </div>

        <div className="flex gap-4 p-4">
           <div className="justify-start border-white border-2 items-center">
            <p className="text-center">Week Performance</p>
            <ChartContainer config={chartConfig} className="min-h-[200px] w-full">
                <BarChart accessibilityLayer data={chartData}>
                  <CartesianGrid vertical={false} />
                  <XAxis
                    dataKey="month"
                    tickLine={false}
                    tickMargin={10}
                    axisLine={false} 
                    tickFormatter={(value)=> value.slice(0,3)}
                  />
                  <ChartTooltip content={<ChartTooltipContent />} />
                  <ChartLegend content={<ChartLegendContent />} />
                  <Bar dataKey="Visits" fill="var(--color-Visits)" radius={4} />
                  <Bar dataKey="Orders" fill="var(--color-Orders)" radius={4} />
                </BarChart>
              </ChartContainer>
            </div> 
            <div className="justify-end border-white border-2">
              <Calendar
                mode="single"
                selected={date}
                onSelect={setDate}
                className="rounded-lg border"
              />
            </div>
        </div>
          <div>
            <h1>
              Today is: {today.toDateString()}<br/>
              {today.getDate()}
              </h1> 
            </div> 
        <SidebarTrigger className="border-2 border-white absolute bottom-5 ml-auto w-40 h-10"/>
      </main>
    </SidebarProvider>
  )
}