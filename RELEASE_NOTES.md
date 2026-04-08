# Release Notes - Unofficial Allegro .NET SDK

> **Note:** This is an unofficial, community-maintained SDK. It is not officially endorsed or supported by Allegro.

## Version 2.1.0 (March 2026)

This release adds Allegro Prices / Alle Discount management and Marketplace information retrieval, bringing total API coverage to 97%+.

### What's New

**Allegro Prices & Alle Discount (`AllegroPricesClient` - 12 methods)**  
Full support for automated pricing programs and discount campaigns:
- **Consent Management:** Get and update per-offer Allegro Prices consent across marketplaces
- **Account Eligibility & Consent:** Check program eligibility and manage account-level consent
- **Alle Discount Campaigns:** List available discount campaigns
- **Eligible & Submitted Offers:** Query offers eligible for, or already enrolled in, discount campaigns
- **Submit / Withdraw Commands:** Submit or withdraw offers from discount campaigns and poll command status

**Marketplaces (`MarketplacesClient` - 1 method)**  
New client exposing marketplace configuration data:
- `GetAllMarketplacesAsync` – Retrieve details for all Allegro marketplaces (supported languages, currencies, and shipping countries)

### Improvements

- API coverage increased to 97%+ (185+ of 190 endpoints)
- Added 13 new API methods across 2 new clients
- Expanded models: `AllegroPricesOfferConsentResponse`, `AllegroPricesAccountEligibility`, `AllegroPricesAccountConsent`, `AlleDiscountCampaigns`, `AlleDiscountEligibleOffers`, `AlleDiscountSubmittedOffers`, `AlleDiscountCommandResponse`, `AllegroMarketplaces`

### Technical Details

**New Clients:**
- `AllegroPricesClient`: 12 methods
- `MarketplacesClient`: 1 method

**Totals:**
- Methods: 170+ → 185+ (+13)
- Clients: 33 → 35 (+2)

### Compatibility

This release is fully backward compatible with v2.0.0. No code changes are required when upgrading.

---

## Version 2.0.0 (October 2025)

This release focuses on warehouse management and marketplace features, bringing API coverage to 95%.

### What's New

**Fulfillment Support**  
Added comprehensive support for Allegro Fulfillment through the new `FulfillmentClient` (17 methods):
- Create and manage Advance Ship Notices (ASN)
- Track inventory with filtering and sorting
- Retrieve order parcels and available products
- Configure tax IDs for international shipping
- Set removal preferences

**Marketplace Features**  
Three new clients provide access to promotional and discovery tools:
- `ListingClient` - Search public offers by phrase, category, or seller (4 methods)
- `BadgesClient` - Create and manage badge campaigns (6 methods)
- `ClassifiedsClient` - Handle classifieds packages and assignments (4 methods)

**Enhanced Communication**  
- `DisputeAttachmentsClient` - Upload and download binary files for disputes (3 methods)
- Extended `SaleExtensionsClient` with additional service management (6 methods)
- Extended `AdvancedOffersClient` with offer attachment support (3 methods)
- Added fundraising campaign search to `MiscellaneousClient`

### Improvements

- API coverage increased from 86% to 95% (170+ of 180 endpoints)
- Added 44 new API methods across 5 new clients
- Extended `AllegroHttpClient` with binary data handling methods
- Expanded test suite to over 200 unit tests
- Complete XML documentation for all new APIs

### Technical Details

**New Clients:**
- FulfillmentClient: 17 methods
- ListingClient: 4 methods  
- BadgesClient: 6 methods
- ClassifiedsClient: 4 methods
- DisputeAttachmentsClient: 3 methods

**Extended Clients:**
- SaleExtensionsClient: +6 methods
- AdvancedOffersClient: +3 methods
- MiscellaneousClient: +1 method

**Totals:**
- Methods: 150 → 170+ (+20)
- Clients: 30 → 35 (+5)
- Model Classes: +80

### Compatibility

This release is fully backward compatible with v1.4.0. No code changes are required when upgrading.

## Version 1.4.0 (October 2025)

### New Features
- ✨ Added Compatibility List management (4 new methods) - Essential for automotive parts sellers
- ✨ Added Offer Events monitoring - Real-time tracking of offer changes
- ✨ Added CPS Conversions tracking - Affiliate program support
- ✨ Added Deposit Types retrieval
- ✨ Added Auction Bidding support
- ✨ Added Matching Categories with scoring

### Improvements
- 🎯 Increased API coverage from 81% to 86% (150/174 endpoints)
- 🐛 Fixed all build warnings - now 0 errors, 0 warnings
- ✅ All 105 unit tests passing (100%)
- 📝 Enhanced XML documentation for all new methods
- 🚀 Added 9 new model classes for new endpoints

### API Coverage
- **MiscellaneousClient:** 5 → 14 methods (+9)
- **ProductClient:** 4 → 5 methods (+1)
- **Total Methods:** 141 → 150+ (+9)

## Version 1.3.0 (October 2025)

### New Features
- Added Post-Purchase Issues management (5 methods)
- Added Refund Claims management (4 methods)
- Added Additional Emails management (4 methods)
- Added Contacts management (5 methods)
- Added Size Tables management (4 methods)
- Added Responsible Persons/Producers clients (GPSR compliance)

### Improvements
- Increased API coverage to 81%
- Added 34 new methods
- Enhanced order management capabilities

## Version 1.2.0 (October 2025)

### New Features
- Added Shipment Management (13 methods) - Create shipments, labels, protocols, pickups
- Added Batch Operations (9 methods) - Bulk price/quantity/modification changes
- Added Customer Returns (3 methods)

### Improvements
- Increased API coverage to ~75%
- Added 25 new methods
- Improved logistics capabilities

## Version 1.1.0 (October 2025)

### New Features
- Added After-Sales Services (12 methods) - Return policies, warranties, implied warranties
- Added Points of Service (5 methods) - Pickup locations
- Added User Ratings (5 methods) - Reputation management
- Added Messaging (5 methods) - Buyer-seller communication
- Added Disputes management (4 methods)

### Improvements
- Increased API coverage to ~65%
- Added 31 new methods
- Enhanced customer service capabilities

## Version 1.0.0 (October 2025)

### Initial Release
- 🎉 Core API implementation with 82 methods
- ✅ Offer Management (17 methods)
- ✅ Product Management (2 methods)
- ✅ Order Management (2 methods)
- ✅ Category Management (3 methods)
- ✅ Image Upload (3 methods)
- ✅ Pricing & Fees (1 method)
- ✅ Shipping & Delivery (7 methods)
- ✅ Payments & Billing (5 methods)
- 🛡️ 11 specialized exception types
- 🔄 Built-in retry logic with exponential backoff
- 📝 Full XML documentation
- ✅ 70+ unit tests
