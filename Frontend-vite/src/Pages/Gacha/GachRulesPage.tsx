import { SidebarProvider } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/app-sidebar"

export default function GachaRulesPage() {
  return (
    <SidebarProvider>
      <AppSidebar />

      <main className="flex-1 p-6 max-w-2xl">
        <h1 className="text-2xl font-bold text-center">Gacha Rules and Disclaimers:</h1>

        <p className ="mt-4">
            We wanted to create a fun and engaging reward system for our customers,
            so we developed a Gacha-like mechanism that allows customers to test their luck 
            and win prizes based on their spending. This Gacha system is designed 
            to add an element of excitement and surprise to the shopping experience
            at BB Food Truck. And if you are not lucky today, don't worry! We also combined 
            fortune coockies with the Gacha system, so you can still get some fun and intersting
            fortune coockies with every purchase, regardless of the Gacha outcome.
        </p>

        <p className="mt-2">
          For every <strong>$25</strong> spent,
          a customer gets <strong>1 Gacha roll</strong>.
        </p>

        <div>
          <h3 className="text-lg font-semibold mb-2 mt-6">
            Drop Rate on Count of 30
          </h3>

          <table className="border border-gray-300 text-sm">
              <tr>
                <th className="border px-3 py-2">Rarity</th>
                <th className="border px-3 py-2">Expected Rate</th>
                <th className="border px-3 py-2">Count (of 30)</th>
                <th className="border px-3 py-2">Actual Rate (%)</th>
              </tr>
            <tbody>
              <tr>
                <td className="border px-3 py-2">Common</td>
                <td className="border px-3 py-2">50%</td>
                <td className="border px-3 py-2">15</td>
                <td className="border px-3 py-2">50%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Rare</td>
                <td className="border px-3 py-2">30%</td>
                <td className="border px-3 py-2">9</td>
                <td className="border px-3 py-2">30%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Epic</td>
                <td className="border px-3 py-2">12%</td>
                <td className="border px-3 py-2">3</td>
                <td className="border px-3 py-2">10%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Mystic</td>
                <td className="border px-3 py-2">7%</td>
                <td className="border px-3 py-2">2</td>
                <td className="border px-3 py-2">6.66%</td>
              </tr>
              <tr>
                <td className="border px-3 py-2">Legendary</td>
                <td className="border px-3 py-2">1%</td>
                <td className="border px-3 py-2">1</td>
                <td className="border px-3 py-2">3.33%</td>
              </tr>
            </tbody>
          </table>

        </div>
      </main>
    </SidebarProvider>
  )
}