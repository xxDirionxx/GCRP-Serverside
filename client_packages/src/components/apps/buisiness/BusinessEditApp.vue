<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Business</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-header>Message Of The Day</v-ons-list-header>
            <v-ons-list-item>
                <input v-model="newMOTD" placeholder="MOTD eingeben" />
            </v-ons-list-item>
        </v-ons-list>
        <section>
            <v-ons-button class="color-background green" modifier="large" v-on:click="saveMOTD">Speichern</v-ons-button><br/><br/>
            <v-ons-button class="color-background red" modifier="large" v-on:click="leaveBusiness">Verlassen</v-ons-button>
        </section>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "BusinessEditApp",
        data() {
            return {
                newMOTD: ""
            }
        },
        methods: {
            leaveBusiness() {
                this.triggerServer("leaveBusiness")
                this.pageStack.splice(1, this.pageStack.length - 1)
            },
            responseBusinessMOTD(businessMOTD) {
                this.oldMOTD = JSON.parse(businessMOTD)
            },
            saveMOTD() {
                if(this.newMOTD == "") return
                this.triggerServer("saveBusinessMOTD", this.newMOTD)
            }
        },
        props: ['pageStack']
    })
</script>

<style lang="scss" scoped>
    section {
        margin: 20px;
    }
</style>