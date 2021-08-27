<template>
    <v-ons-page>
        <ons-toolbar>
            <div class="left">
                <ons-toolbar-button @click="prev()">
                    <ons-icon icon="md-chevron-left"></ons-icon>
                </ons-toolbar-button>
            </div>
            <div class="center">Wallpaper</div>
            <div class="right" style="margin-left: -1vh;">
                <ons-toolbar-button @click="next()">
                    <ons-icon icon="md-chevron-right"></ons-icon>
                </ons-toolbar-button>
            </div>
        </ons-toolbar>
        <ons-carousel fullscreen swipeable auto-scroll overscrollable id="carousel">
            <ons-carousel-item v-for="wallpaper in wallpapers" :key="wallpaper.id" v-bind:style="{ 'background-image': 'url(' + backgroundImage(wallpaper.file) + ')' }" style="background-position:center center;background-size:cover;">
                <div style="text-align: center; font-size: 30px; margin-top: 20px; color: #fff;">
                    <span>{{wallpaper.name}}</span>
                </div>
                <v-ons-fab modifier="mini" position='bottom right' v-on:click="saveWallpaper(wallpaper.id, wallpaper.file)">
                    <ons-icon icon="fa-floppy-o"></ons-icon>
                </v-ons-fab>
            </ons-carousel-item>
        </ons-carousel>
        <v-ons-fab modifier="mini" position='bottom left' v-on:click="pageBack">
            <v-ons-icon icon="md-arrow-left"></v-ons-icon>
        </v-ons-fab>
    </v-ons-page>
</template>

<script>
    import Apps from "../../apps"

    export default Apps.register({
        name: "SettingsEditWallpaperApp",
        data() {
            return {
                wallpapers: [],
                selectedWallpaper: null,
                selectedFile: ""
            }
        },
        methods: {
            responseWallpaperList(wallpapers) {
                this.wallpapers = JSON.parse(wallpapers)
            },
            prev() {
                var carousel = document.getElementById('carousel');
                carousel.prev();
            },
            next() {
                var carousel = document.getElementById('carousel');
                carousel.next();
            },
            saveWallpaper(id, file) {
                this.triggerServer("saveWallpaper", id)
                this.triggerServer("requestPhoneWallpaper")
                //this.trigger("saveWallpaper",[])
            },
            pageBack() {
                this.pageStack.pop()
            },
            backgroundImage(file) {
                return require('@/assets/smartphone/wallpaper/' + file)
            }
        },
        mounted() {
            this.triggerServer("requestWallpaperList")
        },
        props: ['pageStack']
    })
</script>

<style lang="scss" scoped>
    section {
        margin: 20px;
    }
</style>