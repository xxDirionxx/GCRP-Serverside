<template>
    <v-ons-page>
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button class="pointerClass"></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Mitglied verwalten: {{ member.name }}</div>
        </v-ons-toolbar>
        <v-ons-card>
            <div class="content">
                <v-ons-row style="font-size: 20px; color: black; margin-top: 2rem;">
                    <v-ons-col width="10%">Rang:</v-ons-col>
                    <v-ons-col>
                        <input placeholder="Rang" number max="12" min="0" step="1" v-model="member.rang" value="member.rang" style="width: 100%;">
                    </v-ons-col>
                </v-ons-row>
                <v-ons-row style="font-size: 20px; color: black; margin-top: 1rem;">
                    <v-ons-col width="10%">Titel:</v-ons-col>
                    <v-ons-col>
                        <input placeholder="Title" type="text" v-model="member.title" value="member.title" style="width: 100%;">
                    </v-ons-col>
                </v-ons-row>
                <v-ons-row style="font-size: 20px; color: black; margin-top: 1rem;">
                    <v-ons-col width="10%">Payday:</v-ons-col>
                    <v-ons-col>
                        <input placeholder="Payday" number step="100" min="0" v-model="member.payday" value="member.payday" style="width: 100%;">
                    </v-ons-col>
                </v-ons-row>
                <div style="margin-top: 3rem; padding-bottom: 3rem; text-align: center;">
                    <v-ons-button style="float:left;" modifier="medium" v-on:click="saveMember()">Speichern</v-ons-button>
                    <v-ons-button class="color-background red" style="margin-left: 20px; float:left;" modifier="medium" v-on:click="kickMember()">Rauswerfen</v-ons-button>
                </div>
            </div>
        </v-ons-card>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "FraktionEditApp",
        data() {
            return {
                member: [],
            }
        },
        methods: {
            saveMember() {
                if (this.member.rang < 0 || this.member.rang > 12) return
                if (this.member.payday < 0 || this.member.payday > 99000) return
                this.triggerServer("editFraktionMember", this.member.id, this.member.rang, this.member.payday, this.member.title)
                this.pageStack.pop()
            },
            kickMember() {
                if(this.member.rang == 12) return
                this.triggerServer("kickFraktionMember", this.member.id, this.member.rang)
            }
        },
        mounted() {
            
        },
        props: ['pageStack']
    })
</script>

<style lang="scss" scoped>
    section {
        margin: 20px;
    }

    .fRow {
        padding: 1rem 0 0 1rem;
    }

    .mittig {
        text-align: center;
    }

    .pointerClass {
        cursor: pointer;
    }
</style>