#!/usr/bin/env pwsh
# Quick Setup Script for FoodWeb Secrets
# Run this script after rotating your credentials in Azure and Stripe

Write-Host "🔐 FoodWeb Secrets Setup" -ForegroundColor Cyan

# Initialize user secrets for OrderAPI
Write-Host "`n📝 Setting up User Secrets for Food.Services.OrderAPI..."
Set-Location "Food.Services.OrderAPI"
dotnet user-secrets init --force
Write-Host "✓ User Secrets initialized"

# Get connection string from user
$serviceBusConnection = Read-Host "Enter your Azure Service Bus Connection String"
if ($serviceBusConnection) {
	dotnet user-secrets set "ConnectionStrings:ServiceBusConnection" $serviceBusConnection
	Write-Host "✓ Azure Service Bus connection string stored" -ForegroundColor Green
} else {
	Write-Host "⚠ Skipped Azure Service Bus connection string" -ForegroundColor Yellow
}

# Get Stripe key from user
$stripeKey = Read-Host "Enter your Stripe Secret Key (sk_test_... or sk_live_...)"
if ($stripeKey) {
	dotnet user-secrets set "Stripe:SecretKey" $stripeKey
	Write-Host "✓ Stripe Secret Key stored" -ForegroundColor Green
} else {
	Write-Host "⚠ Skipped Stripe Secret Key" -ForegroundColor Yellow
}

# Setup ShoppingCartAPI
Write-Host "`n📝 Setting up User Secrets for Food.Services.ShopingCartAPI..."
Set-Location "../Food.Services.ShopingCartAPI"
dotnet user-secrets init --force
Write-Host "✓ User Secrets initialized"

if ($serviceBusConnection) {
	dotnet user-secrets set "ConnectionStrings:ServiceBusConnection" $serviceBusConnection
	Write-Host "✓ Azure Service Bus connection string stored" -ForegroundColor Green
}

# Verify setup
Write-Host "`n✓ Verification" -ForegroundColor Cyan
Write-Host "User Secrets for Food.Services.OrderAPI:"
Set-Location "../Food.Services.OrderAPI"
dotnet user-secrets list

Write-Host "`n✅ Setup Complete! You can now run the application locally." -ForegroundColor Green
Write-Host "`nNext steps:"
Write-Host "1. Commit changes to git (secrets are NOT included)"
Write-Host "2. Run: dotnet run"
Write-Host "3. For production, set environment variables in your deployment platform"
