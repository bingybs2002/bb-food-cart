import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Field,
  FieldDescription,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Link, useNavigate } from "react-router-dom"
import { useState } from "react"

export function LoginForm({
  className,
  ...props
}: React.ComponentProps<"form">) {
  const [phoneNumber, setPhoneNumber] = useState("")
  const [password, setPassword] = useState("")
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState("")
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError("")
    setIsSubmitting(true)

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/account/login`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            phoneNumber,
            password,
          }),
        },
      )

      const data = await response.json()

      if (!response.ok) {
        setError(data.message ?? "Login failed.")
        return
      }

      localStorage.setItem("accessToken", data.accessToken)
      localStorage.setItem("accessTokenExpire", data.accessTokenExpireAtUtc)
      localStorage.setItem("userId", data.userId)
      navigate("/homepage")
    } catch {
      setError("Unable to reach the server. Please try again.")
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form
      className={cn("flex flex-col gap-6", className)}
      onSubmit={handleSubmit}
      {...props}
    >
      <FieldGroup>
        <div className="flex flex-col items-center gap-1 text-center">
          <h1 className="text-2xl font-bold">Login to your account</h1>
          <p className="text-sm text-balance text-muted-foreground">
            Enter your phone below to login to your account
          </p>
        </div>

        <Field>
          <FieldLabel htmlFor="phone">Phone</FieldLabel>
          <Input
            id="phone"
            type="tel"
            placeholder="1234567833"
            required
            className="bg-background"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
          />
        </Field>

        <Field>
          <div className="flex items-center">
            <FieldLabel htmlFor="password">Password</FieldLabel>
            <a
              href="#"
              className="ml-auto text-sm underline-offset-4 hover:underline"
            >
              Forgot your password?
            </a>
          </div>
          <Input
            id="password"
            type="password"
            required
            className="bg-background"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </Field>

        {error ? (
          <FieldDescription className="text-center text-red-500">
            {error}
          </FieldDescription>
        ) : null}

        <Field>
          <Button type="submit" disabled={isSubmitting}>
            {isSubmitting ? "Logging in..." : "Login"}
          </Button>
        </Field>

        <Field>
          <FieldDescription className="text-center">
            Don&apos;t have an account?{" "}
            <Link to="/signup" className="underline-offset-4 hover:underline">
              Sign up
            </Link>
          </FieldDescription>
        </Field>
      </FieldGroup>
    </form>
  )
}
