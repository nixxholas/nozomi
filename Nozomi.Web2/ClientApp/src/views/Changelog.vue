<template>
    <div>
        <section class="hero">
            <div class="hero-body">
                <div class="container">
                    <h1 class="title">
                        Changelog
                    </h1>
                    <div class="timeline">
                        <header class="timeline-header">
                            <span class="tag is-danger">Public Preview</span>
                        </header>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">8 December 2019 - <b>preview 1</b></p>
                                <ul>
                                    <li>Currency view UI improvements</li>
                                    <li>Currency-specific market pairs listing</li>
                                    <li>Compute engine general fixes and improvements</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">14 December 2019 - <b>preview 2</b></p>
                                <ul>
                                    <li>Currency view UI hotfixes</li>
                                    <li>Compute engine critical bug fixes and increased safetynet checks</li>
                                    <li>Compute engine - Fully compatible with Nozomi's complete rewrite</li>
                                    <li>Role-based Authorisation Control (RBAC) implementation</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">19 December 2019 - <b>preview 3</b></p>
                                <ul>
                                    <li>Dashboard - Requests table UI improvements</li>
                                    <li>Dashboard - Requests modification is now implemented.</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">21 December 2019 - <b>preview 4</b></p>
                                <ul>
                                    <li>General hotfixes and improvements across Nozomi Web</li>
                                    <li>Currency Pairs Creation Interface</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">23 December 2019 - <b>preview 5</b></p>
                                <ul>
                                    <li>.NET Core 3.1 framework upgrade</li>
                                    <li>Prepare for currency sources</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-primary">
                            <div class="timeline-marker is-primary"></div>
                            <div class="timeline-content">
                                <p class="heading">29 December 2019 - <b>preview 6</b></p>
                                <ul>
                                    <li>.NET Core 3.1 framework compatibility update and hotfixing</li>
                                    <li>Backend sorting is now working as intended</li>
                                    <li>We literally added a huge supercharger to every part of th e app</li>
                                    <li>Some user interface elements finally get to work</li>
                                </ul>
                            </div>
                        </div>
                        <header class="timeline-header">
                            <span class="tag is-info">Public Beta</span>
                        </header>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">03 January 2020 - <b>beta 1</b></p>
                                <ul>
                                    <li><b>UI</b> - Our pricing plans are out bitches</li>
                                    <li><b>UI</b> - General hotfixes and improvements</li>
                                    <li><b>UI</b> - Well.. we couldn't really create currency pairs but now we can</li>
                                    <li><b>Cabin</b> - Components listing for requests</li>
                                    <li><b>Cabin</b> - Components for a request can now be created</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">11 January 2020 - <b>beta 2</b></p>
                                <ul>
                                    <li><b>Backend</b> - Complete database wipe for MVP preparations</li>
                                    <li><b>Backend</b> - Certain entities now carry a more obscure ID</li>
                                    <li><b>Backend</b> - Package updates for better .NET Core 3.1 compatibility</li>
                                    <li><b>UI</b> - General hotfixes and improvements</li>
                                    <li><b>Cabin</b> - Currency type management</li>
                                    <li><b>Cabin</b> - Source type and Currency type tables now have a self-update functionality</li>
                                    <del><li><b class="has-text-danger">Known Issue</b> - Some creation forms send a UI console error after the creation of an entity is complete</li></del>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">12 January 2020 - <b>beta 3</b></p>
                                <ul>
                                    <li><b>Backend</b> - Hotfixes for obscure ID implementations</li>
                                    <li><b>Backend</b> - Buffed the search function for currency pair's search API</li>
                                    <li><b>Cabin</b> - The currency pair input box for creating a request is now working as intended</li>
                                    <li><b>Documentation</b> - API legal terms page introduction</li>
                                    <li><b>UI</b> - Slowly updating the look of it..</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">13 January 2020 - <b>beta 4</b></p>
                                <ul>
                                    <li><b>API</b> - The data obscurity increases ever so slightly ;))</li>
                                    <li><b>Backend</b> - Compute engine general deployment hotfixes</li>
                                    <li><b>Cabin</b> - A request's analysed components can now be seen</li>
                                    <li><b>Cabin</b> - A request analysed components can now be created</li>
                                    <li><b>Cabin</b> - A request analysed components can now be modified</li>
                                    <strike><li><b class="has-text-danger">Known Issue</b> - Nozomi API Documentation is down for now.. We have some deprecated crap clashing with our new stuff</li></strike>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">16 January 2020 - <b>beta 5</b></p>
                                <ul>
                                    <li><b>UI</b> - We're getting started on user profile modification features!</li>
                                    <li><b>Auth</b> - You can now register and login with your email or username!</li>
                                    <li><b>Auth</b> - New framework compliance updates</li>
                                    <li><b>Auth</b> - Cancelling login finally works now</li>
                                    <li><b>Auth</b> - Email verification functionality; <i>yes click the link to verify your account</i></li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">23 January 2020 - <b>beta 6</b></p>
                                <ul>
                                    <li><b>UI</b> - Password updating functionality</li>
                                    <li><b>UI</b> - View account information</li>
                                    <li><b>Auth</b> - Non-Web3-based accounts now require email verification</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">27 January 2020 - <b>beta 7</b></p>
                                <ul>
                                    <li><b>UI</b> - View account information</li>
                                    <li><b>UI</b> - Account Information updating functionality</li>
                                    <li><b>Auth</b> - Manual logout redirection implementation</li>
                                    <li><b>Web</b> - Framework & rendering engine updates</li>
                                    <li><b>Web Docs</b> - We got it working again</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">30 January 2020 - <b>beta 8</b></p>
                                <ul>
                                    <li><b>Auth</b> - All connections to Nozomi are now end-to-end encrypted</li>
                                    <li><b>Web</b> - All connections to Nozomi are now end-to-end encrypted</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">04 February 2020 - <b>beta 9</b></p>
                                <ul>
                                    <li><b>Auth</b> - General UI updates</li>
                                    <li><b>Web</b> - Setup payments for your account! </li>
                                    <li><b>Web</b> - Add a card for payments </li>
                                    <li><b>Web</b> - Remove a card for payments! <i>Of course you gotta have more than one to do that.</i> </li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">08 February 2020 - <b>beta 10</b></p>
                                <ul>
                                    <li><b>Web</b> - Actual initial plans out! </li>
                                    <li><b>Web</b> - General hotfixes and improvements to the payment flow </li>
                                    <li><b>Web</b> - Subscribe to a plan! </li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">14 February 2020 - <b>beta 11</b></p>
                                <ul>
                                    <li><b>Web</b> - Infrastructure solidification, rock solid base right now! </li>
                                    <li><b>Web</b> - Request properties are now available </li>
                                    <li><b>Web</b> - General hotfixes and improvements to the request user interface </li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">15 February 2020 - <b>beta 12</b></p>
                                <ul>
                                    <li><b>Web/Dashboard</b> - Source types are now auto-syncing! <i><b>Every actions results in refresh</b></i> </li>
                                    <li><b>Web/Dashboard</b> - Finally managed to resolve the duplicate column issue for requests listing</li>
                                    <li><b>Web</b> - Some redundant elements were cleared up so that you get less worthless content </li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-info">
                            <div class="timeline-marker is-info"></div>
                            <div class="timeline-content">
                                <p class="heading">15 March 2020 - <b>beta 13</b></p>
                                <ul>
                                    <li><b>Web</b> - Rate limiting is up!</li>
                                    <li><b>Web</b> - We've now shifted the market indices to the demo page, preparing our "what we do" at the homepage!</li>
                                    <li><b>Web</b> - You can now delete requests</li>
                                    <li><b>Web</b> - You can now create websocket commands</li>
                                    <li><b>Web</b> - You can now delete websocket commands</li>
                                    <li><b>Web</b> - You can now update websocket commands</li>
                                    <li><b>Web</b> - You can now add websocket command properties</li>
                                    <li><b>Web</b> - You can now delete websocket command properties</li>
                                    <li><b>Web</b> - You can now update websocket command properties</li>
                                    <li><b>Web</b> - Bugs and issues tracker is now online!</li>
                                    <li><b>Auth</b> - Resend email confirmation support</li>
                                    <li><b>Auth</b> - General User interface improvements</li>
                                    <li><b>Auth</b> - Well, we made our emails presentable ;)</li>
                                    <li><b>Auth</b> - More security patches</li>
                                    <li><b>API</b> - We've reworked how you can interact with APIs to make them reliable and resilient.</li>
                                    <li><b>Compute</b> - We've just wrapped up our computation engine at the back. UI 
                                    will be introduced soon! This beast can compute any mathematical formula you can think of!</li>
                                    <li><b>Backend</b> - Optimisation works on how we poll RESTful endpoints; We're handling 429 errors gracefully now :)</li>
                                    <li><b>Backend</b> - Email confirmation no longer requires a re-login to reflect in your profile</li>
                                </ul>
                            </div>
                        </div>
                        <header class="timeline-header">
                            <span class="tag is-primary">Early Access Production (EAP)</span>
                        </header>
                        <div class="timeline-item is-warning">
                            <div class="timeline-marker is-warning"></div>
                            <div class="timeline-content">
                                <p class="heading">21 March 2020 - <b>EAP 1</b></p>
                                <ul>
                                    <li><b>Web</b> - New home page?</li>
                                    <li><b>Auth</b> - API Key generation support</li>
                                    <li><b>API</b> - All GET APIs are up!</li>
                                    <li><b>API</b> - Documentations are slowly being pushed as we speak :)</li>
                                </ul>
                            </div>
                        </div>
                        <div class="timeline-item is-warning">
                            <div class="timeline-marker is-warning"></div>
                            <div class="timeline-content">
                                <p class="heading">23 March 2020 - <b>EAP 2</b></p>
                                <ul>
                                    <li><b>Web</b> - API Key management</li>
                                    <li><b>Auth</b> - API Keys are now visually shortened</li>
                                    <li><b class="has-text-danger">Known Issue</b> - Nozomi API is down for a complete documentation rewrite.</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
    export default {
        name: "changelog",
        data: function() {
            return {}
        }
    }
</script>

<style scoped>

</style>