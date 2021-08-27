<template>
    <v-ons-page class="settingsringtone">
        <v-ons-toolbar>
            <div class="left">
                <v-ons-toolbar-button>
                    <v-ons-back-button></v-ons-back-button>
                </v-ons-toolbar-button>
            </div>
            <div class="center">Klingeltoene</div>
        </v-ons-toolbar>
        <v-ons-list>
            <v-ons-list-item v-if="ringtones.length == 0">
                <div class="center">Keine Klingeltoene vorhanden</div>
            </v-ons-list-item>
            <v-ons-list-item v-if="ringtone" v-for="ringtone in ringtones" :key="ringtone.id" tappable v-on:click="setCurrentRingtone(ringtone)" :value="ringtone.id" v-model="selectedRingtoneId">
                <label class="left">
                    <v-ons-radio :input-id="'radio-' + ringtone.id" :value="ringtone.id" v-model="selectedRingtoneId"></v-ons-radio>
                </label>
                <label :for="'radio-' + ringtone.id" class="center">
                    {{ ringtone.name }}
                </label>
            </v-ons-list-item>
        </v-ons-list>
        <v-ons-list modifier="inset">
            <v-ons-list-item v-if="selectedRingtoneId != -1">
                {{selectedRingtoneName}}
                <audio autoplay ref="media" currentTime="0" type="audio/ogg" preload="auto" :src="getRingtone()"/>
            </v-ons-list-item>
            <v-ons-list-item>
                <v-ons-button modifier="large" @click="saveRingtone">Speichern</v-ons-button>
            </v-ons-list-item>
        </v-ons-list>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"
    import Ringtone from "../../sounds/ringtones"
    import Ringtone2 from "../../sounds/ringtones2"

    export default Apps.register({
        name: "SettingsEditRingtonesApp",
        data() {
            return {
                ringtones: [],
                selectedRingtoneId: 0,
                selectedRingtoneName: "",
            }
        },
        methods: {
            responseRingtoneList(data) {
                this.ringtones = JSON.parse(data).ringtones;
                this.setCurrentRingtone(0);
            },
            getRingtone() {     
                if(this.selectedRingtoneId >= 9 ){
                    return Ringtone2.ringtone(this.selectedRingtoneId)
                }
                else return Ringtone.ringtone(this.selectedRingtoneId)
            },
            setCurrentRingtone(ringtone) {
                this.selectedRingtoneId = ringtone.id;
                this.selectedRingtoneName = ringtone.name;
            },
            saveRingtone() {
                this.triggerServer("saveRingtone", this.selectedRingtoneId)
                this.pageStack.pop()
            },
        },
        mounted() {
            this.triggerServer("requestRingtoneList")
        },
        props: ['pageStack']
    })
</script>

<style lang="scss" scoped>
    section {
        margin: 20px;
    }

    .settingsringtone {
        .page__background, .page--material__background {
            background-color: rgba(0,0,0,0.9);
        }

        .list--material {
            background-color: rgba(0,0,0,0.5);
            color: #ccc;
        }
        .list-item {
            border: 0px;
            background-color: rgba(0,0,0,0.8);
            color: #ccc;

        }
        .list--inset {
            border: 0px;
            margin: 0;
        }
        .list-item__center {
            background: none;
        }
    }
</style>