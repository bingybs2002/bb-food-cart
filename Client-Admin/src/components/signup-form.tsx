"use client"

import * as React from "react"
import { Link } from "react-router-dom"

import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Field,
  FieldDescription,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"

export function SignupForm({
  className,
  ...props
}: React.ComponentProps<"form">) {

  const [form, setForm] = React.useState({
    fullName: "",
    phoneNumber: "",
    password: "",
    confirmPassword: "",
    streetAddress: "ADMINISTRATOR",
    city: "ADMINISTRATOR",
    state: "ADMINISTRATOR",
    zipCode: "ADMINISTRATOR",
  })

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({
      ...form,
      [e.target.id]: e.target.value,
    })
  }

  //handles the register api.
const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault()

  if (form.password !== form.confirmPassword) {
    alert("Passwords do not match")
    return
  }

  const payload = {
    phoneNumber: form.phoneNumber,
    password: form.password,
    cosignee: form.fullName, // backend name
    allergies: 0, // admin = no allergies
    streetAddress: "ADMINISTRATOR",
    city: "ADMINISTRATOR",
    state: "ADMINISTRATOR",
    zipCode: "ADMINISTRATOR",
  }

  const res = await fetch(
    `${import.meta.env.VITE_API_BASE_URL}/account/register`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    }
  )

  if (!res.ok) {
    const msg = await res.text()
    alert(msg)
    return
  }

  alert("Admin account created")

  if(res.ok){
    window.location.href="/login"
  }
}
  return (
    <form 
    onSubmit={handleSubmit}
    className={cn("flex flex-col gap-6", className)} {...props}>
      <FieldGroup>
        <div className="flex flex-col items-center gap-1 text-center">
          <h1 className="text-2xl font-bold">Create your account</h1>
          <p className="text-sm text-balance text-muted-foreground">
            Fill in the form below to create your account.
          </p>
        </div>

        <Field>
          <FieldLabel>Full Name</FieldLabel>
          <Input
            id="fullName"
            type="text"
            placeholder="John Doe"
            required
            className="bg-background"
            value={form.fullName}
            onChange={handleChange}
          />
        </Field>

        <Field>
          <FieldLabel htmlFor="phoneNumber">Phone</FieldLabel>
          <Input
            id="phoneNumber"
            type="tel"
            placeholder="xxx-xxx-xxxx"
            required
            className="bg-background"
            value={form.phoneNumber}
            onChange={handleChange}
          />
          <FieldDescription>
            We will not share your information with anyone else. Your data will
            be kept secure with us.
          </FieldDescription>
        </Field>

        <Field>
          <FieldLabel htmlFor="password">Password</FieldLabel>
          <Input
            id="password"
            type="password"
            required
            className="bg-background"
            value={form.password}
            onChange={handleChange}
          />
          <FieldDescription>
            Must be at least 4 characters long.
          </FieldDescription>
        </Field>

        <Field>
          <FieldLabel htmlFor="confirmPassword">Confirm Password</FieldLabel>
          <Input
            id="confirmPassword"
            type="password"
            required
            className="bg-background"
            value={form.confirmPassword}
            onChange={handleChange}
          />
          <FieldDescription>Please confirm your password.</FieldDescription>
        </Field>

       

        <Field>
          <Button type="submit">Create Account</Button>
        </Field>

        <Field>
          <FieldDescription className="px-6 text-center">
            Already have an account?{" "}
            <Link to="/login" className="underline-offset-4 hover:underline">
              Sign in
            </Link>
          </FieldDescription>
        </Field>
      </FieldGroup>
    </form>
  )
}